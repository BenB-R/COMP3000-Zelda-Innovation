using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public class PlayerManager : MonoBehaviour
    {
        public int health = 100;
        public bool canGlide = false;

        public void UnlockGlide()
        {
            Debug.Log("here too?");
            canGlide = true;
            // Notify the PlayerController or any other component that needs to know about this change.
        }
    }
}
