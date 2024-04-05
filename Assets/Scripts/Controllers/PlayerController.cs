using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool glideActivated = false;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float interactionDistance = 3f;

    [Header("Gliding")]
    [SerializeField] private GameObject glideObject; // The object to toggle for gliding
    [SerializeField] private bool canGlide = false;
    [SerializeField] private float glideFallSpeed = 2f; // Reduced falling speed
    [SerializeField] private float glideMoveSpeed = 7f; // Increased movement speed while gliding

    #region getters
    public bool CanGlide { get; private set; }

    #endregion getters

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (glideObject != null)
            glideObject.SetActive(false); // Ensure the glide object is initially disabled
    }

    private void Update()
    {
        HandleGliding();
    }

    public void MovePlayer(Vector3 direction)
    {
        Vector3 movement = direction;

        if (movement == Vector3.zero)
        {
            // If there is no movement input, smoothly decelerate to a stop
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * moveSpeed);
        }
        else
        {
            // Convert the movement vector to be relative to the camera's orientation
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Flatten the camera vectors on the XZ plane
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate the movement direction relative to the camera's orientation
            Vector3 worldDirection = cameraForward * movement.z + cameraRight * movement.x;

            // Apply the movement to the Rigidbody
            rb.MovePosition(rb.position + worldDirection * moveSpeed * Time.deltaTime);

            // Update the player's rotation to match the movement direction
            if (worldDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(worldDirection);
            }
        }
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            glideActivated = false; // Reset glide activation on jump
            Debug.Log("Jumped");
        }
        else
        {
            Debug.Log("Not Grounded");
        }
    }

    private bool IsGrounded()
    {
        float rayStartHeight = -0.8f;
        float rayLength = 0.25f;
        RaycastHit hit;

        Vector3 rayStart = transform.position + Vector3.up * rayStartHeight;
        Debug.DrawRay(rayStart, Vector3.down * rayLength, Color.red);

        if (Physics.Raycast(rayStart, Vector3.down, out hit, rayLength))
        {
            Debug.Log($"Hit: {hit.collider.gameObject.name}, Point: {hit.point}, Distance: {hit.distance}");
            return true;
        }

        return false;
    }

    private bool IsAtHeight()
    {
        float rayStartHeight = -0.8f;
        float heightCheck = 1f; // Minimum height for gliding

        Vector3 rayStart = transform.position + Vector3.up * rayStartHeight;

        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit, heightCheck))
        {
            // Return true if the player is higher than the specified height
            return hit.distance > heightCheck;
        }
        return true; // Assume true if raycast doesn't hit anything
    }

    public void Interact()
    {
        float raycastHeightOffset = 0.6f; // Vertical offset for the ray start
        float raycastForwardOffset = 0.6f; // Forward offset for the ray start

        // Calculate the ray start point
        Vector3 rayStart = transform.position + Vector3.up * raycastHeightOffset + transform.forward * raycastForwardOffset;
        RaycastHit hit;

        // Draw the ray in the Scene view for debugging
        Debug.DrawRay(rayStart, transform.forward * interactionDistance, Color.red);

        if (Physics.Raycast(rayStart, transform.forward, out hit, interactionDistance))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name); // Log the name of the hit object

            NPCController npc = hit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
                Debug.Log("NPC is not null!");
                // Interact with the NPC
                if (npc.dialogueUI.activeInHierarchy)
                {
                    npc.NextDialogue();
                }
                else
                {
                    npc.StartInteraction();
                }
            }
        }

        if (Physics.Raycast(rayStart, transform.forward, out hit, interactionDistance))
        {
            // ... existing NPC interaction code ...

            AbilityActivator abilityActivator = hit.collider.GetComponent<AbilityActivator>();
            if (abilityActivator != null)
            {
                abilityActivator.ActivateAbility(this);
            }
        }
    }

    private void HandleGliding()
    {
        // Check if the player can glide, is not grounded, and the glide hasn't been activated yet
        if (canGlide && !IsGrounded() && !glideActivated)
        {
            // Activate gliding when the glide condition is met
            glideActivated = true;
        }

        // Handle the gliding physics and visuals
        if (glideActivated)
        {
            // Enable the glide object
            if (glideObject != null)
                glideObject.SetActive(true);

            // Modify the Rigidbody's velocity to reduce falling speed
            Vector3 glideVelocity = rb.velocity;
            glideVelocity.y = -glideFallSpeed;
            rb.velocity = glideVelocity;

            // Increase movement speed while gliding
            moveSpeed = glideMoveSpeed;
        }
        else
        {
            // Disable the glide object and reset gliding parameters when gliding stops
            if (glideObject != null)
                glideObject.SetActive(false);

            // Revert to normal movement speed when not gliding
            moveSpeed = 5f;
        }
    }

    public void UnlockGlide()
    {
        canGlide = true;
    }
}