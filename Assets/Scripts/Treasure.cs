using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public class Treasure : MonoBehaviour
    {
        public enum ContentType { Health, XP, Key, BossKey }
        public ContentType contains; // What does the chest contain?
        public int amount; // Amount of the content
        [SerializeField] private PlayerManager playerManager;

        public void Opened()
        {
            switch (contains)
            {
                case ContentType.Health:
                    Debug.Log("You received " + amount + " Health!");
                    break;
                case ContentType.XP:
                    Debug.Log("You received " + amount + " XP!");
                    break;
                case ContentType.Key:
                    Debug.Log("You received " + amount + " Key(s)!");
                    playerManager.regularKeys++;
                    break;
                case ContentType.BossKey:
                    Debug.Log("You received " + amount + " Key(s)!");
                    playerManager.bossKeys++;
                    break;
            }

            Destroy(gameObject); // Destroy the chest object after it has been opened
        }
    }
}
