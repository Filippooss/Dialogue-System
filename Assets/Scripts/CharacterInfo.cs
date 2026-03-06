using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "Scriptable Objects/CharacterInfo")]
    public class CharacterInfo : ScriptableObject
    {
        public string CharacterName;
        public Sprite CharacterIcon;
        public Color CharacterColor;
    }
}
