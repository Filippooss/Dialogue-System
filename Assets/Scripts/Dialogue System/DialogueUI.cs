using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private Button bt_Skip;
        [SerializeField] private Button bt_Continue;
        [SerializeField] private TMP_Text t_Dialogue;
        [SerializeField] private TMP_Text t_CharacterName;
        [SerializeField] private Image img_CharacterIcon;
        [SerializeField] private CanvasGroup canvasGroup;
        [Tooltip("Time it takes to reveal each character")]
        [SerializeField] private float charRevealSpeed = 0.2f;
        [Tooltip("Delay in seconds applied after characters defined in punctuationChars.")]
        [SerializeField] private float punctuationWaitTime = 0.5f;
        [Tooltip("Don't separate the characters. The final string must contains only the wanted characters")]
        [SerializeField] private string punctuationChars = ",.:";

        private Coroutine dialogueCoroutine = null;
        private string currentLine = string.Empty;
        private bool nextLine = false;


        void Awake()
        {
            bt_Continue.onClick.AddListener(() =>
            {
                ContinueDialogue();
            });

            bt_Skip.onClick.AddListener(() =>
            {
                SkipDialogue();
            });

            //clear
            t_Dialogue.text = "";
        }
        void Start()
        {

        }

        public void ContinueDialogue()
        {
            if (IsWriting())
            {
                StopCoroutine(dialogueCoroutine);
                t_Dialogue.text = currentLine;
            }
            else
            {
                //continue to next line
                nextLine = true;
            }
        }

        public void SkipDialogue()
        {
            if (IsWriting())
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }
            StartCoroutine(SmoothHideRoutine());
            Debug.Log("End of dialogue");
        }

        public void PlayDialogue(SO_DialogueModel dialogueModel, Action dialogueFinished = null)
        {
            StartCoroutine(PlayDialogueRoutine(dialogueModel));
        }

        private IEnumerator PlayDialogueRoutine(SO_DialogueModel dialogueModel)
        {
            for (int i = 0; i < dialogueModel.Conversation.Count; i++)
            {
                dialogueCoroutine = StartCoroutine(WriteDialogueRoutine(
                    dialogueModel.Conversation[i].Line,
                    dialogueModel.Conversation[i].CharacterInfo.CharacterName,
                    dialogueModel.Conversation[i].CharacterInfo.CharacterIcon
                ));

                //last line
                if (i == dialogueModel.Conversation.Count)
                {
                    Debug.Log("last line");
                    yield return null;
                }
                else
                {
                    yield return new WaitUntil(() => !IsWriting() && nextLine);
                    nextLine = false;
                }
            }

            Debug.Log("Dialogue has finished");
        }

        private IEnumerator SmoothHideRoutine(float duration = 0.5f)
        {
            float timer = 0;
            float currentAlpha = canvasGroup.alpha;
            float targetAlpha = 0;
            while (timer < duration)
            {
                timer += Time.deltaTime;

                canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / duration);
                yield return null;
            }

            //make sure we get there
            canvasGroup.alpha = targetAlpha;
        }

        private IEnumerator WriteDialogueRoutine(string line, string characterName, Sprite characterIcon)
        {
            img_CharacterIcon.sprite = characterIcon;
            t_Dialogue.text = string.Empty;
            currentLine = line;
            t_CharacterName.text = characterName;
            foreach (char c in line)
            {
                t_Dialogue.text += c;

                if (punctuationChars.Contains(c) || c == '\n')
                {
                    yield return new WaitForSeconds(punctuationWaitTime);
                }
                else
                {
                    yield return new WaitForSeconds(charRevealSpeed);
                }
            }
        }

        public bool IsWriting()
        {
            return t_Dialogue.text != currentLine;
        }
    }
}

