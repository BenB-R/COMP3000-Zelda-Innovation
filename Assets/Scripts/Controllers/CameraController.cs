using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform target; // The enemy target to lock onto
    public Vector3 offset = new Vector3(0, 3f, -4f);
    public Vector3 combatOffset = new Vector3(0, 4f, -6f);
    public float sensitivity = 5.0f;
    public float minYAngle = -35f;
    public float maxYAngle = 60f;
    public float lockOnSpeed = 5.0f; // Control the speed of lock-on transition
    [Tooltip("Smoothing factor for the camera movement")]
    public float smoothTime = 0.2f; // Smoothing factor, now adjustable in the inspector

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float distance = 4.0f;
    private float lastCombatXAngle = 0.0f;
    private Quaternion lastCombatRotation;
    private bool transitioningOutOfCombat = false;

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public LayerMask collisionLayer;

    private Vector3 velocity = Vector3.zero; // For smooth damping
    private Quaternion rotationVelocity;

    void Start()
    {
        distance = maxDistance;
        lastCombatRotation = Quaternion.identity;
    }

    public void HandleMouseInput(Vector2 mouseMovement)
    {
        currentX += mouseMovement.x * sensitivity;
        currentY -= mouseMovement.y * sensitivity;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target != null)
        {
            LockOnTargetSmooth();
            transitioningOutOfCombat = false; // Ensure we are not transitioning when a target is present
        }
        else
        {
            if (!transitioningOutOfCombat)
            {
                transitioningOutOfCombat = true; // Start transitioning out of combat
                currentX = lastCombatXAngle; // Retain the last horizontal angle from combat
            }

            AdjustDistance();
            RegularCameraMovement();
        }
    }


    private void RegularCameraMovement()
    {
        Vector3 direction = Quaternion.Euler(currentY, currentX, 0) * new Vector3(0, 0, -1);
        Vector3 position = player.position + direction * distance + offset;
        transform.position = position;
        transform.LookAt(player.position + offset);
    }

    public void LockOnTargetSmooth()
    {
        Vector3 directionToTarget = target.position - player.position;
        Vector3 desiredPosition = player.position - directionToTarget.normalized * distance + combatOffset;
        desiredPosition.y = Mathf.Max(desiredPosition.y, player.position.y + combatOffset.y);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lockOnSpeed * Time.deltaTime);

        lastCombatXAngle = transform.eulerAngles.y;
        lastCombatRotation = transform.rotation; // Update lastCombatRotation to current rotation in combat
    }

    private void AdjustDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position + offset, -transform.forward, out hit, maxDistance, collisionLayer))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance; // No obstruction, use the maximum distance
        }
    }
}