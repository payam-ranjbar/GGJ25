using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
        private AudioSource _breathSource;
        private int _breathCount;
        [SerializeField] private AudioMixerGroup _group;

        private void Start()
        {
                if(_breathSource is null)
                        _breathSource = gameObject.AddComponent<AudioSource>();

                _breathSource.outputAudioMixerGroup = _group;
                _breathSource.loop = false;
        }

        public AudioSource AudioSource => _breathSource;
        public bool IsReachedMax(int max)
        { 
                return _breathCount >= max;
        }
        
        public void Breathe()
        {
                _breathCount++;

        }

        public void ResetBreathCount()
        {
                _breathCount = 0;
        }
}