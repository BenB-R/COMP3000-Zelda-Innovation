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

    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public LayerMask collisionLayer;

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float distance = 4.0f;
    private float lastCombatXAngle = 0.0f;
    private bool transitioningOutOfCombat = false;

    private Vector3 velocity = Vector3.zero; // For smooth damping

    private void Start()
    {
        distance = maxDistance;
    }

    private void Update()
    {
        HandleMouseInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            HandleCombatCamera();
        }
        else if (transitioningOutOfCombat)
        {
            HandleTransitionOutOfCombat();
        }
        else
        {
            HandleExplorationCamera();
        }

        AdjustDistance();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ExitCombatMode()
    {
        transitioningOutOfCombat = true;
        target = null; // Clear the target
    }

    public void HandleMouseInput(Vector2 mouseMovement)
    {
        // Smooth out mouse input
        currentX = Mathf.Lerp(currentX, currentX + mouseMovement.x * sensitivity, Time.deltaTime * 10f);
        currentY = Mathf.Clamp(Mathf.Lerp(currentY, currentY - mouseMovement.y * sensitivity, Time.deltaTime * 10f), minYAngle, maxYAngle);
    }

    private void HandleCombatCamera()
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

        transitioningOutOfCombat = false; // Reset the flag when in combat
    }

    private void HandleTransitionOutOfCombat()
    {
        // Smoothly transition to the original camera position while maintaining the X angle
        Vector3 dir = Quaternion.Euler(0, lastCombatXAngle, 0) * new Vector3(0, 0, -distance) + offset;
        Vector3 desiredPosition = player.position + dir;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(player.position + offset);

        // Consider resetting transitioningOutOfCombat to false once transition is complete or based on a condition
    }

    private void HandleExplorationCamera()
    {
        // Smooth out camera rotation
        Quaternion targetRotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Smooth out camera position
        Vector3 dir = new Vector3(0, 0, -distance);
        Vector3 desiredPosition = player.position + transform.rotation * dir + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
        transform.LookAt(player.position + offset);
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
}