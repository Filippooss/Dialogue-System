using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float volume;
        private List<AudioSource> audioSources = new();
        [Header("Listening To")]
        [SerializeField] private SO_AudioChannelEvent mainAudioChannel;

        void Awake()
        {
            foreach (Transform child in transform)
            {
                var source = child.GetComponent<AudioSource>();
                source.volume = volume;
                audioSources.Add(source);
            }

            mainAudioChannel.OnEventRaised += HandleMainAudioChannel;
        }

        private void HandleMainAudioChannel(AudioClip clip)
        {
            foreach (AudioSource source in audioSources)
            {
                if (!source.isPlaying)
                {
                    source.clip = clip;
                    source.volume = volume;
                    source.Play();
                    return;
                }
            }
        }
    }
}
