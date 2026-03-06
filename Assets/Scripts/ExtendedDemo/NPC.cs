using UnityEngine;

namespace DialogueSystem
{
    public class NPC : MonoBehaviour
    {
        [SerializeField] private SO_DialogueData dialogueModel;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private InteractionPrompt interactionPrompt;
        public void Interact()
        {
            //prevent player from moving
            inputReader.DisablePlayerInput();
            DialogueManager.Instance.PlayDialogue(dialogueModel, () =>
            {
                //enable input after dialogue finished
                inputReader.EnablePlayerInput();
            });
        }

        public void ShowPrompt()
        {
            interactionPrompt.Show();
        }

        public void HidePrompt()
        {
            interactionPrompt.Hide();
        }
    }
}
