using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    /// <summary>
    /// Helping class for testing the DialogueUI 
    /// </summary>
    public class DialogueTestUI : MonoBehaviour
    {
        [SerializeField] private Button bt_Restart;
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private SO_DialogueData testDialogue;
        [SerializeField] private InputReader inputReader;
        private Transform container;

        void Awake()
        {
            container = transform.GetChild(0);
            bt_Restart.onClick.AddListener(() =>
            {
                TestDialogue();
            });
            Hide();
        }

        void Start()
        {
            inputReader.EnableDialogueInput();
            TestDialogue();
        }

        private void TestDialogue()
        {
            Hide();
            dialogueUI.PlayDialogue(testDialogue, () =>
            {
                inputReader.DisableDialogueInput();
                Show();
            });
        }

        private void Show()
        {
            container.gameObject.SetActive(true);
        }

        private void Hide()
        {
            container.gameObject.SetActive(false);
        }

    }
}
