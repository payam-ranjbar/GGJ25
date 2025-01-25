using System;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
        private AudioSource _breathSource;
        private int _breathCount;

        private void Start()
        {
                if(_breathSource is null)
                        _breathSource = gameObject.AddComponent<AudioSource>();

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