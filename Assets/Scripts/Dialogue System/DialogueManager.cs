using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    /// <summary>
    /// This class is responsible for managing the dialogue system. Furthermore it acts as middle man between this system and 
    /// other systems in the game
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private InputReader inputReader;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning($"More than one {nameof(DialogueSystem)} exists in the scene");
                Destroy(gameObject);
            }

            inputReader.ContinueEvent += HandleContinue;
            inputReader.SkipEvent += HandleSkip;
            inputReader.InputControllerChangeEvent += HandleInputControllerChange;

            //disable dialogue input when game starts
            inputReader.DisableDialogueInput();
        }

        public void PlayDialogue(SO_DialogueData dialogueModel, Action onFinish)
        {
            inputReader.EnableDialogueInput();
            dialogueUI.PlayDialogue(dialogueModel, onFinish);
        }

        private void HandleSkip()
        {
            inputReader.DisableDialogueInput();
            dialogueUI.SkipDialogue();
        }

        private void HandleContinue()
        {
            dialogueUI.ContinueDialogue();
        }
        private void HandleInputControllerChange(E_InputMethod method)
        {
            dialogueUI.ChangeInput(method);
        }

        void OnDestroy()
        {
            inputReader.ContinueEvent -= HandleContinue;
            inputReader.SkipEvent -= HandleSkip;
        }
    }
}
