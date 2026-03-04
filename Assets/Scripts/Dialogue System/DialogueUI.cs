using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [Tooltip("Time it takes to reveal each character")]
        [SerializeField] private float charRevealSpeed = 0.2f;
        [Tooltip("Delay in seconds applied after characters defined in punctuationChars.")]
        [SerializeField] private float punctuationWaitTime = 0.5f;
        [Tooltip("Don't separate the characters. The final string must contains only the wanted characters")]
        [SerializeField] private string punctuationChars = ",.:";

        private Coroutine dialogueCoroutine = null;
        private string currentLine = string.Empty;



        void Awake()
        {
            bt_Continue.onClick.AddListener(() =>
            {
                if (t_Dialogue.text != currentLine)
                {
                    StopCoroutine(dialogueCoroutine);
                    t_Dialogue.text = currentLine;
                }
                else
                {
                    //continue to next line
                }
            });

            bt_Skip.onClick.AddListener(() =>
            {
                Debug.Log("End of dialogue");
            });

            //clear
            t_Dialogue.text = "";
        }

        void Start()
        {
            dialogueCoroutine = StartCoroutine(WriteDialogueRoutine("Reprehenderit laboris consequat anim esse dolore duis enim ex aliqua excepteur dolor do. Sint minim culpa consectetur sint elit ullamco ut dolore Lorem. Mollit Lorem adipisicing non incididunt nostrud eu amet officia officia qui incididunt et. Aliqua laboris tempor ea deserunt. Velit ea pariatur minim adipisicing minim et aliqua aliqua in dolor adipisicing. Ea quis nulla culpa sit ullamco id. Ut ipsum deserunt aute occaecat minim eu incididunt enim nulla eu labore commodo."));
        }

        private IEnumerator WriteDialogueRoutine(string line)
        {
            currentLine = line;
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
    }
}

