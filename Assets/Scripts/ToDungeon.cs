using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace fyp
{
    public class ToDungeon : MonoBehaviour
    {
        public GameObject triggerObject; // Public GameObject to assign the trigger collider

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player has entered the trigger, loading Dungeon scene.");
                SceneManager.LoadScene("Dungeon");
            }
            else
            {
                Debug.Log("Non-player object entered the trigger: " + other.gameObject.name);
            }
        }
    }
}
