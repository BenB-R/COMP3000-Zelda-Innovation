using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpForce = 5f;
    private Rigidbody rb;
    [SerializeField]
    private bool canGlide;
    [SerializeField]
    private float interactionDistance = 3f;

    #region getters
    public bool CanGlide { get; private set; }

    #endregion getters

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        // Create a new Vector3 to hold the direction based on direct key presses
        Vector3 directInput = new Vector3(
            Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            0,
            Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
        );

        // If there was no input from the input system (i.e., direction is zero), use the direct input
        Vector3 movement = (direction == Vector3.zero) ? directInput : direction;

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
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        /*
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jumped");
        }
        else
        {
            Debug.Log("Not Grounded");
        }*/
    }

    private bool IsGrounded()
    {
        float extraHeight = 5f;
        RaycastHit hit;
        // LayerMask to specify which layer we are hitting. Replace "TerrainLayerName" with the actual name of your terrain layer.
        LayerMask groundLayer = LayerMask.GetMask("Terrain");

        // Visualize the raycast in the editor for debugging purposes
        Debug.DrawRay(transform.position + Vector3.up * extraHeight, Vector3.down * (extraHeight + 0.1f), Color.red);

        // Cast a ray downward from just below the bottom of the player object to detect "ground"
        if (Physics.Raycast(transform.position + Vector3.up * extraHeight, Vector3.down, out hit, extraHeight + 0.1f, groundLayer))
        {
            // If the ray hits something on the ground layer, the player is grounded
            return true;
        }

        return false;
    }


    public void Interact()
    {
        float raycastHeightOffset = 1.0f; // Adjust this value as needed
        Vector3 raycastStartPosition = transform.position + Vector3.up * raycastHeightOffset;
        RaycastHit hit;

        if (Physics.Raycast(raycastStartPosition, transform.forward, out hit, interactionDistance))
        {
            NPCController npc = hit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
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
    }


    public void UnlockGlide()
    {
        CanGlide = true;
    }

    // You can add additional methods here for other abilities
}
