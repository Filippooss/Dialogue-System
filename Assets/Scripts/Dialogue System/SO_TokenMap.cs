using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "SO_TokenMap", menuName = "Scriptable Objects/Dialogue System/SO_TokenMap")]
    public class SO_TokenMap : SO_Description
    {
        public List<TokenMap> TokenMapList;
    }

    [System.Serializable]
    public class TokenMap
    {
        public string RawToken;
        [SerializeField] private string glyphSpriteName;
        public string GlyphSpriteName
        {
            get
            {
                return $"<sprite name=\"{glyphSpriteName}\">";
            }
            private set
            {
                glyphSpriteName = value;
            }
        }
    }
}
