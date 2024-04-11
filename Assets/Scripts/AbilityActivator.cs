using UnityEngine;
using TMPro;
using fyp;

public class AbilityActivator : MonoBehaviour
{
    public enum AbilityType { Glide, Sprint, JumpBoost }

    [SerializeField]
    private AbilityType abilityToActivate;

    public string abilityName; // The name to display when the ability is found
    [SerializeField]
    private TextMeshProUGUI messageText; // Reference to a TextMesh Pro UGUI element

    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false); // Hide the message text initially
    }

    public void ActivateAbility(PlayerManager playerManager)
    {
        Debug.Log("do it get here?");
        switch (abilityToActivate)
        {
            case AbilityType.Glide:
                playerManager.UnlockGlide();
                break;
                // Add cases for other abilities here
        }

        if (MessageManager.Instance != null)
        {
            MessageManager.Instance.ShowMessage("You Unlocked the " + abilityName + " ability!!", 5f);
        }
        Destroy(gameObject); // Destroy the object immediately
    }
}