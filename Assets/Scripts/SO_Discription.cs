using UnityEngine;

namespace DialogueSystem
{
    public abstract class SO_Description : ScriptableObject
    {
        [SerializeField, TextArea] private string description;
    }
}
