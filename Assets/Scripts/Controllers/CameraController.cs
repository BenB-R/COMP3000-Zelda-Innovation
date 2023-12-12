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

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity;
        currentY -= Input.GetAxis("Mouse Y") * sensitivity;
        currentY = Mathf.Clamp(currentY, -35, 60);

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
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.position + rotation * dir;
        transform.LookAt(player.position);
    }
}
