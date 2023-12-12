using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    public static NPCController ActiveNPC; // Static reference to track the active NPC

    public string npcName; // Variable to store the NPC's name
    public string[] dialogues; // Array to store multiple dialogues
    public GameObject dialogueUI;
    public TextMeshProUGUI nameText; // Text element for the NPC's name
    public TextMeshProUGUI dialogueText;
    private int currentDialogueIndex = 0; // To track the current dialogue

    private void Start()
    {
        dialogueUI.SetActive(false);
    }

    public void StartInteraction()
    {
        if (dialogues.Length == 0) return; // Check if there are any dialogues

        // If another NPC is currently active, close its dialogue
        if (ActiveNPC != null && ActiveNPC != this)
        {
            ActiveNPC.HideDialogue();
        }

        // Set this NPC as the active one
        ActiveNPC = this;
        ShowDialogue();
    }

    public void NextDialogue()
    {
        // If the current NPC is not the active one, do nothing
        if (ActiveNPC != this) return;

        // Update the dialogue index for next interaction
        currentDialogueIndex = (currentDialogueIndex + 1) % dialogues.Length;

        // If there are no more dialogues, hide the dialogue box
        if (currentDialogueIndex == 0)
        {
            HideDialogue();
            ActiveNPC = null; // Reset the active NPC reference
        }
        else
        {
            // Otherwise, show the next dialogue
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        nameText.text = npcName; // Set the NPC's name
        dialogueText.text = dialogues[currentDialogueIndex]; // Set current dialogue
        dialogueUI.SetActive(true); // Show the dialogue UI
    }

    public void HideDialogue()
    {
        dialogueUI.SetActive(false); // Hide the dialogue UI
        ActiveNPC = null; // Reset the active NPC reference
    }
}
