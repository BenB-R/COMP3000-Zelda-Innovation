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
    public float smoothTime = 0.2f; // Smoothing factor

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float distance = 4.0f;
    private float lastCombatXAngle = 0.0f;
    private bool transitioningOutOfCombat = false;


    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public LayerMask collisionLayer;

    private Vector3 velocity = Vector3.zero; // For smooth damping

    void Start()
    {
        distance = maxDistance;
    }

    // Normal state camera movement
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

    // Combat state camera movement
    public void HandleCombatCamera(Vector2 lookInput)
    {

        // This example will clamp the Y input and only allow the camera to rotate vertically within limits
        currentY -= lookInput.y * sensitivity;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (target != null) // Target locked, adjust camera for combat
        {
            LockOnTargetSmooth();
            transitioningOutOfCombat = false; // Reset the flag when in combat
        }
        else if (transitioningOutOfCombat)
        {
            // Smoothly transition to the original camera position while maintaining the X angle
            Vector3 dir = Quaternion.Euler(0, lastCombatXAngle, 0) * new Vector3(0, 0, -distance) + offset;
            Vector3 desiredPosition = player.position + dir;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            transform.LookAt(player.position + offset);

            // Consider resetting transitioningOutOfCombat to false once transition is complete or based on a condition
        }
        else
        {
            // Default free camera movement
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 desiredPosition = player.position + rotation * dir + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
            transform.LookAt(player.position + offset);
        }

        AdjustDistance();
    }

    public void LockOnTargetSmooth()
    {
        Vector3 directionToTarget = target.position - player.position;
        Vector3 desiredPosition = player.position - directionToTarget.normalized * distance + combatOffset; // Use combatOffset here
        desiredPosition.y = Mathf.Max(desiredPosition.y, player.position.y + combatOffset.y);

        // Smoothly move the camera to the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Smoothly rotate the camera to look at the target
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lockOnSpeed * Time.deltaTime);
        lastCombatXAngle = transform.eulerAngles.y;
    }

    private void AdjustDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.position, transform.position - player.position, out hit, maxDistance, collisionLayer))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
    }

    public void ExitCombatMode()
    {
        transitioningOutOfCombat = true;
        target = null; // Clear the target
    }
}
