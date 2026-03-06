using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    /// <summary>
    /// This class is responsible for controlling the Ui behavior
    /// </summary>
    public class DialogueUI : MonoBehaviour, IInputChange
    {
        [Serializable]
        public class SpriteAssetPack
        {
            public TMP_SpriteAsset SpriteAsset;
            public E_InputMethod InputMethod;
        }
        [Header("Setup")]
        [SerializeField] private Button bt_Skip;
        [SerializeField] private Button bt_Continue;
        [SerializeField] private TMP_Text t_Dialogue;
        [SerializeField] private TMP_Text t_CharacterName;
        [SerializeField] private Image img_CharacterIcon;
        [SerializeField] private Image img_CharacterIconBackground;
        [SerializeField] private CanvasGroup canvasGroup;
        //we use this for hide/show instead of this.gameobject,
        //so coroutines can work
        [SerializeField] private GameObject dialogueContainer;
        [Header("Configure")]
        [Tooltip("Time it takes to reveal each character")]
        [SerializeField] private float charRevealSpeed = 0.2f;
        [Tooltip("Delay in seconds applied after characters defined in punctuationChars.")]
        [SerializeField] private float punctuationWaitTime = 0.5f;
        [Tooltip("Don't separate the characters. The final string must contains only the wanted characters")]
        [SerializeField] private string punctuationChars = ",.:";
        [SerializeField] private List<SpriteAssetPack> spriteAssetPackList;
        [SerializeField] private SO_TokenMap tokenMap;
        [SerializeField] private AudioClip[] writerSoundArray;
        [Header("Broadcast To")]
        [SerializeField] private SO_AudioChannelEvent mainAudioChannel;

        private Coroutine writerCoroutine = null;
        private string currentLine = string.Empty;
        private bool nextLine = false;
        private bool isSkipped = false;
        // private Dictionary<string, string> tokenMap = new Dictionary<string, string>()
        // {
        //     { "[interact]", "<sprite name=\"interact\">" },
        //     { "[continue]", "<sprite name=\"continue\">" },
        //     { "[skip]", "<sprite name=\"skip\">" }
        // };


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

            //initialize
            Hide();
            canvasGroup.alpha = 0;
        }

        public void ChangeInput(E_InputMethod inputMethod)
        {
            t_Dialogue.spriteAsset = spriteAssetPackList.First(a => a.InputMethod == inputMethod).SpriteAsset;
        }

        public void ContinueDialogue()
        {
            if (IsWriting())
            {
                //stops the writer
                StopCoroutine(writerCoroutine);
                //instant show line
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
            StopCoroutine(writerCoroutine);
            writerCoroutine = null;
            isSkipped = true;
        }

        public void PlayDialogue(SO_DialogueData dialogueModel, Action dialogueFinished = null)
        {
            isSkipped = false;
            //setup visuals before UI is revealed
            var characterInfo = dialogueModel.Conversation[0].CharacterInfo;
            img_CharacterIcon.sprite = characterInfo.CharacterIcon;
            img_CharacterIconBackground.color = characterInfo.CharacterColor;
            t_Dialogue.text = string.Empty;
            t_CharacterName.text = characterInfo.CharacterName;

            StartCoroutine(SmoothShowRoutine(showComplete: () =>
            {
                StartCoroutine(PlayDialogueRoutine(dialogueModel, dialogueFinished));
            }));
        }

        private IEnumerator PlayDialogueRoutine(SO_DialogueData dialogueModel, Action dialogueFinished = null)
        {
            //loop throw the conversation list 
            for (int i = 0; i < dialogueModel.Conversation.Count; i++)
            {
                writerCoroutine = StartCoroutine(WriteDialogueRoutine(
                    ParseLineTokens(dialogueModel.Conversation[i].Line),
                    dialogueModel.Conversation[i].CharacterInfo
                ));

                //last line
                if (i == dialogueModel.Conversation.Count - 1)
                {
                    while (IsWriting())
                    {
                        if (isSkipped)
                        {
                            break;
                        }

                        yield return null;
                    }
                }
                else
                {
                    while (IsWriting() || !nextLine)
                    {
                        //player skipped the dialogue
                        if (isSkipped)
                        {
                            break;
                        }

                        yield return null;
                    }
                    //reset
                    nextLine = false;
                }

                if (isSkipped) break;
            }

            // Debug.Log("Dialogue has finished");
            //no need to wait if skipped
            if (!isSkipped)
            {
                //give the player some time to read
                yield return new WaitForSeconds(2f);
            }

            StartCoroutine(SmoothHideRoutine());

            dialogueFinished?.Invoke();
        }

        #region  Hide
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

            Hide();
        }

        private void Hide()
        {
            dialogueContainer.SetActive(false);
        }
        #endregion

        #region Show
        private void Show()
        {
            dialogueContainer.SetActive(true);
        }

        private IEnumerator SmoothShowRoutine(float duration = 0.5f, Action showComplete = null)
        {
            if (dialogueContainer.activeInHierarchy == false)
                Show();

            float timer = 0;
            float currentAlpha = canvasGroup.alpha;
            float targetAlpha = 1;
            while (timer < duration)
            {
                timer += Time.deltaTime;

                canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / duration);
                yield return null;
            }

            //make sure we get there
            canvasGroup.alpha = targetAlpha;
            showComplete?.Invoke();
        }


        #endregion

        private string ParseLineTokens(string line)
        {
            string processed = line;
            //replace all tokens with sprite names
            foreach (var token in tokenMap.TokenMapList)
            {
                processed = processed.Replace(token.RawToken, token.GlyphSpriteName);
            }

            return processed;
        }
        private IEnumerator WriteDialogueRoutine(string line, CharacterInfo characterInfo)
        {
            currentLine = line;
            //setup UI for next speaker
            img_CharacterIcon.sprite = characterInfo.CharacterIcon;
            img_CharacterIconBackground.color = characterInfo.CharacterColor;
            t_Dialogue.text = string.Empty;
            t_CharacterName.text = characterInfo.CharacterName;

            for (int i = 0; i < line.Length; i++)
            {
                //we found a token
                if (line[i] == '<')
                {
                    string token = string.Empty;

                    for (int j = i; j < line.Length; j++)
                    {
                        token += line[j];
                        if (line[j] == '>')
                        {
                            //skip the token characters
                            i = j + 1;
                            break;
                        }
                    }
                    //add the hole token to the dialogue
                    t_Dialogue.text += token;

                    if (i >= line.Length - 1) break;
                }

                //add the character to the UI
                t_Dialogue.text += line[i];
                //play random audio
                mainAudioChannel.RaiseEvent(writerSoundArray[UnityEngine.Random.Range(0, writerSoundArray.Length)]);

                //determine how much should wait
                if (punctuationChars.Contains(line[i]) || line[i] == '\n')
                {
                    yield return new WaitForSeconds(punctuationWaitTime);
                }
                else
                {
                    yield return new WaitForSeconds(charRevealSpeed);
                }

                //Debug.Log("Writer Finished");
            }
        }

        public bool IsWriting()
        {
            return t_Dialogue.text != currentLine;
        }
    }
}

