using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace fyp
{
    public class PlayerManager : MonoBehaviour
    {
        public float maxHealth = 100;
        public float health = 100;
        public float experience = 0;
        public bool canGlide = false;
        public int level = 0;
        public float experienceNeeded = 100;

        [Header("UI Elements")]
        public Image healthBar;
        public Image experienceBar;
        public TextMeshProUGUI levelText;

        private void Update()
        {
            UpdateHealthBar();
            UpdateExperienceBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = health / maxHealth;
            }
        }

        private void UpdateExperienceBar()
        {
            if (experienceBar != null)
            {
                experienceBar.fillAmount = experience / experienceNeeded;
            }

            if (levelText != null)
            {
                levelText.text = "Level " + level;
            }
        }

        public void UnlockGlide()
        {
            canGlide = true;
        }

        public void GainExperience(float experienceAmount)
        {
            experience += experienceAmount;
            if (experience >= experienceNeeded)
            {
                LevelUp();
            }
        }

        public void Die()
        {
            experience = experience * 0.25f;
            if (level > 1)
            {
                LevelDown();
            }
            health = maxHealth;
            Debug.Log("Player died");
        }

        public void TakeDamage(float amount)
        {
            health -= amount;

            if (health <= 0)
            {
                Die();
            }
        }

        private void LevelUp()
        {
            maxHealth *= 1.1f;
            health = maxHealth;
            experienceNeeded *= 1.2f;
            experience = 0;
            level++;
        }

        private void LevelDown()
        {
            maxHealth /= 1.1f;
            health = maxHealth;
            experienceNeeded /= 1.2f;
            experience = experienceNeeded / 2;
            level--;
        }
    }
}
