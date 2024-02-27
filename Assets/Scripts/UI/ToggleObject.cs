using UnityEngine;

namespace fyp
{
    public class ToggleObject : MonoBehaviour
    {
        [SerializeField]
        private GameObject objectToToggle; // Assign this in the inspector

        // This method is called when the button is clicked
        public void ToggleActiveState()
        {
            if (objectToToggle != null)
            {
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            }
        }
    }
}
