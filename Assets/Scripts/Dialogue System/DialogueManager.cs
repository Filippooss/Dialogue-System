using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private List<SO_GameAction> gameActionsList;
        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private InputReader inputReader;
        [SerializeField] private List<SO_DialogueModel> dialogueModelList;

        void Awake()
        {
            inputReader.ContinueEvent += HandleContinueEvent;
            inputReader.SkipEvent += HandleSkipEvent;
        }

        void Start()
        {
            dialogueUI.PlayDialogue(dialogueModelList[0]);
        }

        private void HandleSkipEvent()
        {
            dialogueUI.SkipDialogue();
        }

        private void HandleContinueEvent()
        {
            dialogueUI.ContinueDialogue();
        }

        void OnDestroy()
        {
            inputReader.ContinueEvent -= HandleContinueEvent;
            inputReader.SkipEvent -= HandleSkipEvent;
        }
    }

    public enum TextTokens
    {
        Interact,

    }

}
