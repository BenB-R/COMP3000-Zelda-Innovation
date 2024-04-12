using fyp;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{

    [Header("Combat")]
    [SerializeField] private Transform cameraTransform; // Assign the main camera's transform here
    [SerializeField] private LayerMask targetLayer; // Set this to the layer your enemies are on
    [SerializeField] private float targetingRange = 15f; // How far in front of the player we check for targets
    [SerializeField] private float angleLimit = 45f; // Angle range for finding targets in front of the player
    [SerializeField] private CameraController cameraController;

    [Header("other stuff")]
    public static PlayerController Instance;
    private Rigidbody rb;
    private bool glideActivated = false;
    private bool jumpReleasedAfterJumping = false;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private GameObject glideObject;
    [SerializeField] private float glideFallSpeed = 2f;
    [SerializeField] private float glideMoveSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float interactionDistance = 3f;
    private bool isSprinting = false;

    private bool isLockedOn = false;
    public Transform currentTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (glideObject != null) glideObject.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        HandleGliding();
        HandleCombat();
    }

    public void OnJumpButtonReleased()
    {
        jumpReleasedAfterJumping = true;
    }

    public void MovePlayer(Vector3 direction)
    {
        if (direction == Vector3.zero)
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * moveSpeed);
        else
            ApplyMovement(direction);
    }

    private void ApplyMovement(Vector3 direction)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = cameraRight.y = 0;
        Vector3 worldDirection = (cameraForward * direction.z + cameraRight * direction.x).normalized;
        rb.MovePosition(rb.position + worldDirection * moveSpeed * Time.deltaTime);
        if (worldDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation(worldDirection);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            glideActivated = jumpReleasedAfterJumping = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position + Vector3.up * -0.8f, Vector3.down, out hit, 0.25f);
    }

    private void HandleGliding()
    {
        if (playerManager != null && playerManager.canGlide && !IsGrounded() && jumpReleasedAfterJumping && Input.GetButton("Jump"))
            ToggleGlide(true);
        else if (IsGrounded() || !Input.GetButton("Jump"))
            ToggleGlide(false);

        if (glideActivated)
        {
            rb.velocity = new Vector3(rb.velocity.x, -glideFallSpeed, rb.velocity.z);
            moveSpeed = glideMoveSpeed;
        }
        else if (!isSprinting)
            moveSpeed = 5f; // Default move speed
    }

    private void ToggleGlide(bool activate)
    {
        glideActivated = activate;
        if (glideObject != null) glideObject.SetActive(activate);
    }

    public void Sprint()
    {
        moveSpeed = sprintSpeed;
        isSprinting = true;
    }

    public void StopSprint()
    {
        if (!glideActivated)
        {
            moveSpeed = 5f; // Default move speed
            isSprinting = false;
        }
    }

    public void Interact()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.6f + transform.forward * 0.6f;
        if (Physics.Raycast(rayStart, transform.forward, out hit, interactionDistance))
        {
            AbilityActivator abilityActivator = hit.collider.GetComponent<AbilityActivator>();
            if (abilityActivator != null)
            {
                // Activate the ability if an AbilityActivator is hit
                abilityActivator.ActivateAbility(playerManager);
            }
            else
            {
                // Try to get an NPCController component from the hit object
                NPCController npc = hit.collider.GetComponent<NPCController>();
                if (npc != null)
                {
                    // Call the new Interact method
                    npc.Interact();
                }
            }
        }
    }


    private bool IsAtHeight()
    {
        float heightCheck = 1f; // Minimum height for gliding
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * -0.8f, Vector3.down, out hit))
        {
            return hit.distance > heightCheck;
        }
        return false; // Assume not at height if raycast doesn't hit anything
    }

    public bool CanLockOntoTarget()
    {
        var target = FindNearestTargetable();
        Debug.Log($"Checking for targets: Found {(target != null ? "one" : "none")}");
        return target != null;
    }


    // Method to toggle lock on
    public void ToggleLockOn()
    {
        if (isLockedOn)
        {
            Debug.Log("Disabling lock-on.");
            isLockedOn = false;
            currentTarget = null;
            cameraController.SetTarget(null); // Clear the target
            LockOnManager.Instance.SetLockOnTarget(currentTarget);
        }
        else
        {
            Debug.Log("Attempting to lock-on.");
            currentTarget = FindNearestTargetable();
            LockOnManager.Instance.SetLockOnTarget(currentTarget);
            if (currentTarget != null)
            {
                Debug.Log($"Locked onto {currentTarget.name}");
                isLockedOn = true;
                cameraController.SetTarget(currentTarget);
            }
            else
            {
                Debug.Log("No valid targets to lock onto.");
            }
        }
    }

    private Transform FindNearestTargetable()
    {
        var targetables = Physics.OverlapSphere(transform.position, targetingRange, targetLayer)
            .Select(hit => hit.transform)
            .Where(t => t.CompareTag("Targetable") && IsInSight(t))
            .OrderBy(t => Vector3.Distance(transform.position, t.position));

        return targetables.FirstOrDefault();
    }

    private bool IsInSight(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        return angleToTarget <= angleLimit;
    }

    public void SwitchTarget(bool switchLeft)
    {
        var targetables = Physics.OverlapSphere(transform.position, targetingRange, targetLayer)
            .Select(hit => hit.transform)
            .Where(t => t.CompareTag("Targetable") && IsInSight(t))
            .OrderBy(t => Vector3.Angle(cameraTransform.forward, t.position - cameraTransform.position));

        if (switchLeft)
        {
            // Find the target to the left of the current target
            currentTarget = targetables.Reverse().SkipWhile(t => t != currentTarget).Skip(1).FirstOrDefault();
        }
        else
        {
            // Find the target to the right of the current target
            currentTarget = targetables.SkipWhile(t => t != currentTarget).Skip(1).FirstOrDefault();
        }

        // If there are no more targets in the desired direction, keep the current target or reset it
        if (currentTarget == null && targetables.Any())
        {
            currentTarget = switchLeft ? targetables.Last() : targetables.First();
        }
    }

    private void HandleCombat()
    {
        if (isLockedOn && currentTarget != null)
        {
            LockOnTarget(currentTarget);
        }
    }

    public void LockOnTarget(Transform target)
    {
        // Player facing target
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Camera facing target
        if (cameraController != null)
        {
            cameraController.SetTarget(currentTarget);
        }
    }

    public void CombatMovement(Vector3 direction, bool isInCombatMode)
    {
        Debug.Log($"Combat Movement Called - Direction: {direction}, IsInCombatMode: {isInCombatMode}");
        Vector3 movement = transform.forward * direction.z + transform.right * direction.x;
        movement *= moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private bool wantsToLockOn = false;

    public void SetWantsToLockOn(bool value)
    {
        wantsToLockOn = value;
    }

    public bool WantsToLockOn()
    {
        return wantsToLockOn;
    }
}