using System;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "AudioChannelEvent", menuName = "Scriptable Objects/Events/SO_AudioChannelEvent")]
    public class SO_AudioChannelEvent : SO_Description
    {
        public Action<AudioClip> OnEventRaised;


        public void RaiseEvent(AudioClip audioClip)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised.Invoke(audioClip);
            }
            else
            {
                Debug.Log($"No subscribers found for event {name}");
            }
        }
    }
}
