using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class CustomButton : MonoBehaviour, IInputChange
    {
        [SerializeField] private Image img_Action;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private SO_GameAction gameAction;
        [SerializeField] private AudioClip pressedSound;
        [Header("Broadcast To")]
        [SerializeField] private SO_AudioChannelEvent mainAudioChannel;
        private Button button;
        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                mainAudioChannel.RaiseEvent(pressedSound);
            });

            inputReader.InputControllerChangeEvent += ChangeInput;
        }

        public void ChangeInput(E_InputMethod inputMethod)
        {
            img_Action.sprite = gameAction.ActionIconsList.First(a => a.Type == inputMethod).Sprite;
        }
    }
}
