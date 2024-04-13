using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public class PlayerManager : MonoBehaviour
    {
        public int health = 100;
        public float experience = 0;
        public bool canGlide = false;
        public int level = 0;

        public void UnlockGlide()
        {
            canGlide = true;
        }

        public void gainExperience(float experienceAmount)
        {
            experience += experienceAmount;
        }

        public void die()
        {
            experience = experience * 0.25f;
        }
    }
}
