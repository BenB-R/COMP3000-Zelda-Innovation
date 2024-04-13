using UnityEngine;

public class LockOnManager : MonoBehaviour
{
    public static LockOnManager Instance { get; private set; }

    public GameObject lockOnMarkerPrefab; // Assign in the inspector, make sure it's a UI element
    private GameObject lockOnMarkerInstance;

    private RectTransform lockOnMarkerRectTransform;
    private Transform currentTarget;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Instantiate the marker but keep it disabled initially
        lockOnMarkerInstance = Instantiate(lockOnMarkerPrefab, GameObject.Find("Canvas").transform);
        lockOnMarkerRectTransform = lockOnMarkerInstance.GetComponent<RectTransform>();
        lockOnMarkerInstance.SetActive(false);
    }

    void Update()
    {
        // If there's a current target, update the marker's position
        if (currentTarget != null)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(GetTargetCenter(currentTarget));

            // Convert the screenPoint to UI space
            lockOnMarkerRectTransform.position = screenPoint;
            lockOnMarkerInstance.SetActive(true);
        }
        else
        {
            lockOnMarkerInstance.SetActive(false);
        }
    }

    public void SetLockOnTarget(Transform newTarget)
    {
        Debug.Log("success! " + newTarget);
        currentTarget = newTarget;
    }

    public void ClearLockOnTarget()
    {
        currentTarget = null;
    }

    Vector3 GetTargetCenter(Transform target)
    {
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider != null)
        {
            // Returns the center of the collider, which is more likely to be the center of the object.
            return targetCollider.bounds.center;
        }

        // Fallback to transform position if no collider is found
        return target.position;
    }

}
