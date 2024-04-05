using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float sensitivity = 5.0f;
    public float minDistance = 1.0f; // Minimum distance from the player
    public float maxDistance = 4.0f; // Maximum distance from the player
    public LayerMask collisionLayer; // Layer to check for collisions

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float distance;

    void Start()
    {
        distance = maxDistance;
    }

    public void HandleMouseInput(Vector2 mouseMovement)
    {
        currentX += mouseMovement.x * sensitivity;
        currentY -= mouseMovement.y * sensitivity;
        currentY = Mathf.Clamp(currentY, -35, 60);
    }

    void LateUpdate()
    {
        // Adjust camera distance if a collision is detected
        RaycastHit hit;
        if (Physics.Raycast(player.position, transform.position - player.position, out hit, maxDistance, collisionLayer))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * dir;
        transform.LookAt(player.position);
    }
}