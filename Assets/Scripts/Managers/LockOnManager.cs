using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public class LockOnManager : MonoBehaviour
    {
        public static LockOnManager Instance { get; private set; }

        public GameObject lockOnMarkerPrefab; // Assign in the inspector
        private GameObject lockOnMarkerInstance;

        private Transform currentTarget;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            // Instantiate the marker but keep it disabled initially
            lockOnMarkerInstance = Instantiate(lockOnMarkerPrefab);
            lockOnMarkerInstance.SetActive(false);
        }

        void Update()
        {
            // If there's a current target, update the marker's position
            if (currentTarget != null)
            {
                lockOnMarkerInstance.transform.position = currentTarget.position;
                lockOnMarkerInstance.SetActive(true);
            }
            else
            {
                lockOnMarkerInstance.SetActive(false);
            }
        }

        public void SetLockOnTarget(Transform newTarget)
        {
            currentTarget = newTarget;
        }

        public void ClearLockOnTarget()
        {
            currentTarget = null;
        }
    }
}
