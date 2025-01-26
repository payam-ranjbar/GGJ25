using System;
using System.Collections;
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
                return RandomUtils.GetRandomIntInRange(0, clips.Length);
        }
}
public class AudioManager : MonoBehaviour
{
        [SerializeField] private AudioSource mainSource;
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource narratorSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private SoundDatabase soundDB;

        [SerializeField] private float delayBetweenClips = 3f;
        [SerializeField] private int breathCountToGasp = 4;

        public static AudioManager Instance { get; private set; }

        public bool startMusicAwake = true;

        private void Awake()
        {
                if (Instance != null && Instance != this)
                {
                        Destroy(gameObject);
                        return;
                }

                Instance = this;
                DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
                PlayAmbient();
                // PlayBGMSequentially();
        }

        public void PlayAmbient()
        {
                ambientSource.loop = false;
                ambientSource.clip = soundDB.GetRandomClipFromSound("ambient");
                ambientSource.Play();
        }
        public void PlayBreathSound(PlayerAudioController playerAudio)
        {

                var source = playerAudio.AudioSource;
                var max = Math.Abs(breathCountToGasp - RandomUtils.GetRandomIntInRange(0, 2));
                
                var maxBreathReached = playerAudio.IsReachedMax(max);

                if (maxBreathReached)
                {
                        PlayGasp(source);
                        PlayBlow(source);
                        playerAudio.ResetBreathCount();
                }
                else
                {
                        PlayBlow(source);
                        playerAudio.Breathe();
                }

        }

        public void PlayNarratorBirds() => PlayNarrator("narrator birds");
        public void PlayNarratorStorm() => PlayNarrator("narrator storm");
        public void PlayNarratorThunder() => PlayNarrator("narrator thunder");
        public void PlayThrust()
        {
                PlayRandomSFX("thrust");
                
        }
        private void PlayGasp(AudioSource source , bool force = false)
        {


                if(source.isPlaying && !force) return;
                var clip = soundDB.GetRandomClipFromSound("gasp");
                source.clip = clip;
                source.Play();
                
        }

        private void PlayBlow(AudioSource source, bool force = false)
        {
                if(source.isPlaying && !force) return;
                source.clip = soundDB.GetRandomClipFromSound("blow");
                
                source.Play();
                
        }


        private void PlayDelayedSFX(string soundName, float delay)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                sfxSource.clip = clip;
                sfxSource.Play();
        }

        private AudioClip PlayRandomSFX(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                sfxSource.PlayOneShot(clip);
                return clip;
        }

        private void PlayNarrator(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                if(narratorSource.isPlaying) return;

                narratorSource.clip = clip;
                narratorSource.Play();

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

                        if (GUILayout.Button("Play BGM"))
                        {
                                PlayBGMSequentially();
                        }
                        if (GUILayout.Button("Play Birds Narrator"))
                        {
                                PlayNarratorBirds();
                        }
                        if (GUILayout.Button("Play Thunder Narrator"))
                        {
                                PlayNarratorThunder();
                        }
                        if (GUILayout.Button("Play Storm Narrator"))
                        {
                                PlayNarratorStorm();
                        }

                GUILayout.EndVertical();
        }
        
        public void PlayBGMSequentially()
        {
                if (soundDB.BgmList.Length == 0)
                {
                        Debug.LogWarning("AudioManager: No BGM clips provided to play sequentially.");
                        return;
                }

                StartCoroutine(PlayBGMCoroutine());
        }

        private IEnumerator PlayBGMCoroutine()
        {
                foreach (AudioClip clip in soundDB.BgmList)
                {
                        if (clip == null)
                        {
                                Debug.LogWarning("AudioManager: Encountered a null clip in BGM array, skipping.");
                                continue;
                        }

                        mainSource.clip = clip;
                        mainSource.Play();
                        yield return new WaitForSeconds(clip.length + delayBetweenClips);
                }

                yield return PlayBGMCoroutine();
        }

}
        
        
        
