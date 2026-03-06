using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "GameAction", menuName = "Scriptable Objects/GameAction")]
    public class SO_GameAction : ScriptableObject
    {

        public List<GameActionIcon> ActionIconsList;
    }

    [Serializable]
    public class GameActionIcon
    {
        public Sprite Sprite;
        public E_InputMethod Type;
    }
}
