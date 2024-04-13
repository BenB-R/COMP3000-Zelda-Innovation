using fyp;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    #region variables
    [Header("Singleton")]
    public static PlayerController Instance;

    [Header("Components")]
    private Rigidbody rb;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;

    [Header("Player State")]
    private bool glideActivated = false;
    private bool jumpReleasedAfterJumping = false;
    private bool isSprinting = false;
    private bool jumpTriggered = false;
    private bool isLockedOn = false;
    private bool smallAttackTriggered = false;
    private bool bigAttackTriggered = false;
    private float attackTimer = 0f;

    [Header("Player Movement Settings")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float glideFallSpeed = 2f;
    [SerializeField] private float glideMoveSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private GameObject glideObject;
    private Vector3 inputDirection; // This will store the current input direction for the player

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;

    [Header("Targeting Settings")]
    public Transform currentTarget;
    [SerializeField] private Transform cameraTransform; // Assign the main camera's transform here
    [SerializeField] private LayerMask targetLayer; // Set this to the layer your enemies are on
    [SerializeField] private float targetingRange = 15f; // How far in front of the player we check for targets
    [SerializeField] private float angleLimit = 90f; // Angle range for finding targets in front of the player
    [SerializeField] private CameraController cameraController;

    [Header("Combat Settings")]
    [SerializeField] private Collider swordHitbox;
    [SerializeField] private float smallAttackDamage = 10f; // Damage dealt by small attack
    [SerializeField] private float bigAttackDamage = 20f; // Damage dealt by big attack
    [SerializeField] private LayerMask enemyLayer; // Layer that enemies are on
    [SerializeField] private float smallAttackCooldown = 1f; // 1 second cooldown for small attack
    [SerializeField] private float bigAttackCooldown = 1.5f; // 1.5 seconds cooldown for big attack

    [Header("Sound Effects")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip bigAttackSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip glideSFX;
    [SerializeField] private AudioClip interactSFX;
    #endregion

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        if (glideObject != null) glideObject.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        UpdateAnimationStates();
        HandleGliding();
        HandleCombat();
    }

    private void UpdateAnimationStates()
    {
        // Existing handling for jump
        if (jumpTriggered)
        {
            animator.SetBool("Jump", true);
            jumpTriggered = false;  // Reset immediately after setting
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        // Continue with existing movement and grounded checks
        bool isMoving = inputDirection.sqrMagnitude > 0.01f;
        bool isGrounded = IsGrounded();
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsSprinting", isSprinting);

        // Handle attack animations using booleans
        animator.SetBool("DoingSmallAttack", smallAttackTriggered);
        animator.SetBool("DoingBigAttack", bigAttackTriggered);
        smallAttackTriggered = false;  // Reset immediately after setting
        bigAttackTriggered = false;    // Reset immediately after setting
    }

    public void OnJumpButtonReleased()
    {
        jumpReleasedAfterJumping = true;
    }

    public void MovePlayer(Vector3 direction)
    {
        if (animator.GetBool("CombatMode")) animator.SetBool("CombatMode", false);

        inputDirection = direction.normalized;
        if (direction == Vector3.zero)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * moveSpeed);
            animator.SetBool("IsMoving", false);
        }
        else
        {
            ApplyMovement(direction);
            animator.SetBool("IsMoving", true);
        }
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
            Debug.Log("Jumped");
            
            // Reset vertical velocity
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.velocity = horizontalVelocity;

            Debug.Log(rb.velocity);
            // Apply the jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);

            // Set flags for jumping
            glideActivated = jumpReleasedAfterJumping = false;
            jumpTriggered = true;

            // Play jump sound effect
            PlaySoundEffect(jumpSFX);
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

    public void ToggleGlide(bool activate)
    {
        glideActivated = activate;
        if (glideObject != null) glideObject.SetActive(activate);
        if (activate) PlaySoundEffect(glideSFX);
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
                PlaySoundEffect(interactSFX);
            }
            else
            {
                // Try to get an NPCController component from the hit object
                NPCController npc = hit.collider.GetComponent<NPCController>();
                if (npc != null)
                {
                    // Call the new Interact method
                    npc.Interact();
                    PlaySoundEffect(interactSFX);
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
        // Get all targetable objects within range and in sight
        var targetables = Physics.OverlapSphere(transform.position, targetingRange, targetLayer)
            .Select(hit => hit.transform)
            .Where(t => t.CompareTag("Targetable") && IsInSight(t))
            .ToList();

        // This will hold the signed angles of each target relative to the player's facing direction
        var targetAngles = targetables.ToDictionary(
            t => t,
            t => Vector3.SignedAngle(cameraTransform.forward, t.position - cameraTransform.position, Vector3.up)
        );

        // Determine the direction of angle change (left is positive, right is negative)
        float directionMultiplier = switchLeft ? 1f : -1f;

        // Sort targets by their angle, adjusted for direction
        var sortedTargets = targetAngles.OrderBy(t => directionMultiplier * t.Value).ToList();

        // Find the index of the current target in the sorted list
        int currentIndex = sortedTargets.FindIndex(t => t.Key == currentTarget);

        // Determine the next target index
        int nextIndex = switchLeft ? (currentIndex + 1) % sortedTargets.Count : (currentIndex - 1 + sortedTargets.Count) % sortedTargets.Count;

        // Set the next target
        currentTarget = sortedTargets[nextIndex].Key;

        // Update the lock-on marker
        LockOnManager.Instance.SetLockOnTarget(currentTarget);
    }

    private void HandleCombat()
    {
        // Update cooldown timers
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

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

    public void CombatMovement(Vector3 direction)
    {
        animator.SetBool("CombatMode", true);

        if (attackTimer <= 0)
        {
            // Assuming the direction is already a local direction relative to the camera or player's forward direction
            Vector3 movement = transform.forward * direction.z + transform.right * direction.x;
            movement *= moveSpeed * Time.deltaTime;
            transform.position += movement;

            // Update animation parameters

            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveZ", direction.z);
        }
        else
        {
            Debug.Log(attackTimer);
            animator.SetFloat("MoveX", 0.0f);
            animator.SetFloat("MoveZ", 0.0f);
        }
    }

    // Method for performing a small attack
    public void PerformSmallAttack()
    {
        if (!smallAttackTriggered && attackTimer <= 0)
        {
            Debug.Log("Performing Small Attack");
            smallAttackTriggered = true;
            ApplyAttackDamage(smallAttackDamage);
            attackTimer = smallAttackCooldown;
            PlaySoundEffect(attackSFX);
        }
    }

    public void PerformBigAttack()
    {
        if (!bigAttackTriggered && attackTimer <= 0)
        {
            Debug.Log("Performing Big Attack");
            bigAttackTriggered = true;
            ApplyAttackDamage(bigAttackDamage);
            attackTimer = bigAttackCooldown;
            PlaySoundEffect(bigAttackSFX);
        }
    }

    // Helper method to apply damage to enemies in range
    private void ApplyAttackDamage(float damage)
    {
        // Check only the collider associated with the sword's hitbox
        Collider[] hitColliders = Physics.OverlapBox(swordHitbox.bounds.center, swordHitbox.bounds.extents, swordHitbox.transform.rotation, LayerMask.GetMask("Default"));
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Targetable"))
            {
                AnimalController animal = hitCollider.GetComponent<AnimalController>();
                if (animal != null)
                {
                    animal.TakeDamage(damage);
                }
            }
        }
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}