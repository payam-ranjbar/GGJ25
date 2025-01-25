using System;
using Scenes;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Sound
{
        [SerializeField] private string name;
        [SerializeField] private AudioClip[] clips;


        public AudioClip[] Clips => clips;
        public int Length => clips.Length;
        

        public AudioClip Pick(int index)
        {
                if (CheckNull()) return null;

                if (OutOfBound(index)) return null;

                return clips[index];
        }

        public AudioClip PickRandom() => Pick(GetRandomIndex());

        public bool IsName(string soundName)
        {
                return String.Equals(soundName, name, StringComparison.CurrentCultureIgnoreCase);
        }
        private bool CheckNull()
        {
                return clips == null || clips.Length == 0;
        }

        private bool OutOfBound(int index)
        {
                return clips.Length <= index;
        }


        private int GetRandomIndex()
        {
                if (Length == 1) return 0;
                return RandomUtils.GetRandomIntInRange(0, clips.Length - 1);
        }
}
public class AudioManager : MonoBehaviour
{
        [SerializeField] private AudioSource mainSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private SoundDatabase soundDB;

        [SerializeField] private int breathCountToGasp = 4;



        public void PlayBreathSound(PlayerAudioController playerAudio)
        {
                var max = Math.Abs(breathCountToGasp - RandomUtils.GetRandomIntInRange(0, 2));
                
                var maxBreathReached = playerAudio.IsReachedMax(max);

                if (maxBreathReached)
                {
                        PlayGasp();
                        playerAudio.ResetBreathCount();
                }
                else
                {
                        PlayBlow();
                        playerAudio.Breathe();
                }

        }

        public void PlayThrust()
        {
                PlayRandomSFX("thrust");
                
        }
        private void PlayGasp()
        {
                PlayRandomSFX("gasp");
        }

        private void PlayBlow()
        {
                PlayRandomSFX("blow");
        }

        

        private void PlayRandomSFX(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                sfxSource.PlayOneShot(clip);
        }
        
        
        
}