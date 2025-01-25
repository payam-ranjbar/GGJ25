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

                var source = playerAudio.AudioSource;
                var max = Math.Abs(breathCountToGasp - RandomUtils.GetRandomIntInRange(0, 2));
                
                var maxBreathReached = playerAudio.IsReachedMax(max);

                if (maxBreathReached)
                {
                        var gaspLength = PlayGasp(source);
                        PlayBlow(source, gaspLength);
                        playerAudio.ResetBreathCount();
                }
                else
                {
                        PlayBlow(source);
                        playerAudio.Breathe();
                }

        }

        public void PlayThrust()
        {
                PlayRandomSFX("thrust");
                
        }
        private float PlayGasp(AudioSource source)
        {
              var clip =  PlayRandomSFX("gasp");
              return clip.length;
        }

        private void PlayBlow(AudioSource source, float gaspLength = 0f, bool force = false)
        {
                if(source.isPlaying && !force) return;
                source.clip = soundDB.GetRandomClipFromSound("blow");
                
                source.PlayDelayed(gaspLength);
                PlayDelayedSFX("blow", gaspLength);
        }


        private void PlayDelayedSFX(string soundName, float delay)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                sfxSource.clip = clip;
                sfxSource.PlayDelayed(delay);
        }

        private AudioClip PlayRandomSFX(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                sfxSource.PlayOneShot(clip);
                return clip;
        }
        
        private void OnGUI()
        { 
                
                PlayerAudioController player1Audio, player2Audio;
                GUILayout.BeginVertical();


                        if (GUILayout.Button("Play Blow Sound for Player 1"))
                        {
                                        player1Audio = GameObject.Find("Player1 Audio").GetComponent<PlayerAudioController>();

                                if (player1Audio == null)
                                { 
                                        GUILayout.Label("Player 1 Audio not found or missing PlayerAudioController component.");
                                        return;
                                }

                                PlayBreathSound(player1Audio);
                        }

                

                        if (GUILayout.Button("Play Blow Sound for Player 2"))
                        {
                       
                                        player2Audio = GameObject.Find("Player2 Audio")
                                                .GetComponent<PlayerAudioController>();
                                

                                if (player2Audio == null)
                                { 
                                        GUILayout.Label("Player 1 Audio not found or missing PlayerAudioController component.");
                                        return;
                                }

                                PlayBreathSound(player2Audio);
                        }

                GUILayout.EndVertical();
        }
}
        
        
        
