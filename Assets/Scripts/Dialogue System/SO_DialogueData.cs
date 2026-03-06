using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/Dialogue System/DialogueData")]
    public class SO_DialogueData : SO_Description
    {
        public List<CharacterLine> Conversation;
    }

    [Serializable]
    public class CharacterLine
    {
        [TextArea] public string Line;
        public CharacterInfo CharacterInfo;
    }
}

