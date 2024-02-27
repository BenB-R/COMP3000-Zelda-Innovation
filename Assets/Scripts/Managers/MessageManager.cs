using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace fyp
{
    public class MessageManager : MonoBehaviour
    {
        public static MessageManager Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI messageText;

        private void Awake()
        {
            // Simple singleton pattern to ensure only one instance exists
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Optional: Make the manager persist across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ShowMessage(string message, float duration)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), duration);
        }

        private void HideMessage()
        {
            messageText.gameObject.SetActive(false);
        }
    }
}
