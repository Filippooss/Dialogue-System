using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private List<SO_GameAction> gameActionsList;

        void Start()
        {

        }

        void Update()
        {

        }
    }

    public enum TextTokens
    {
        Interact,

    }

}
