using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public class Unlockable : MonoBehaviour
    {
        public bool requiresBossKey = false; // Set this if the door requires a boss key to open

        // Call this function with the number of regular keys and boss keys available to the player
        public void OpenAttempt(int regularKeys, int bossKeys)
        {
            if (requiresBossKey)
            {
                if (bossKeys > 0)
                {
                    Destroy(gameObject); // Destroy the door object if a boss key is used
                    Debug.Log("Door opened with a boss key!");
                }
                else
                {
                    Debug.Log("No boss key in inventory.");
                }
            }
            else
            {
                if (regularKeys > 0)
                {
                    Destroy(gameObject); // Destroy the door object if a regular key is used
                    Debug.Log("Door opened with a regular key!");
                }
                else
                {
                    Debug.Log("No key in inventory.");
                }
            }
        }
    }
}
