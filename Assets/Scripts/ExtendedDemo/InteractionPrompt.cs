using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class InteractionPrompt : MonoBehaviour, IInputChange
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private SO_GameAction gameAction;
        [SerializeField] private Image image;

        void Awake()
        {
            inputReader.InputControllerChangeEvent += ChangeInput;
            Hide();
        }

        void Update()
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }

        public void Show()
        {
            image.gameObject.SetActive(true);
        }

        public void Hide()
        {
            image.gameObject.SetActive(false);
        }

        public void ChangeInput(E_InputMethod inputMethod)
        {
            image.sprite = gameAction.ActionIconsList.Find(a => a.Type == inputMethod).Sprite;
        }
    }
}
