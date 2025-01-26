using System;
using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private List<AudioSource> sfxSources;
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private SoundDatabase soundDB;

        [SerializeField] private float delayBetweenClips = 3f;
        [SerializeField] private int breathCountToGasp = 4;

        public static AudioManager Instance { get; private set; }

        public bool startMusicAwake = true;
        
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string musicGroup = "Music";
        [SerializeField] private float maxVolume = 0f;
        [SerializeField] private float minVolume = -80f;
        [SerializeField] private float fadeInDuration = 2f;
        [SerializeField] private float fadeOutDuration = 2f;

        private bool isFading = false;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;

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


        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            PlayerEventHandler.Instance.OnDeath -= PlayDeath;
            PlayerEventHandler.Instance.OnPlayerJoin -= PlayDing;
            PlayerEventHandler.Instance.OnBirdsSpawn -= PlayBirdsSpawn;
        }

        private void Start()
        {
                PlayAmbient();
                // PlayBGMSequentially();
                PlayerEventHandler.Instance.OnDeath += PlayDeath;
                PlayerEventHandler.Instance.OnPlayerJoin += PlayDing;
                PlayerEventHandler.Instance.OnBirdsSpawn += PlayBirdsSpawn;

        }

        public void PlayBirdsSpawn()
        {
                PlaySfx(soundDB.GetRandomClipFromSound("bird spawn"));
        }

        public void PlayNotifDing()
        {
                var clip = soundDB.GetRandomClipFromSound("notif");
                uiSource.PlayOneShot(clip);
        }

        public void PlayGameEnd()
        {
                FadeOut();
                PlayRandomSFX("end game");
        }

        public void PlayDeath()
        {
                PlayRandomSFX("death");
        }

        public void PlayDing()
        {
                PlayRandomSFX("ding");
        }

        public void PlayWinPlayerOne()
        {
                PlayNarrator("player one win");
        }

        public void PlayWinPlayerTwo()
        {
                PlayNarrator("player two win");
 
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
                PlaySfx(clip);
        }

        private AudioClip PlayRandomSFX(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);

                return PlaySfx(clip);
        }

        private AudioClip PlaySfx(AudioClip clip)
        {
                for (var i = 0; i < sfxSources.Count; i++)
                {
                        var source = sfxSources[i];

                        if (!source.isPlaying)
                        {
                                source.clip = clip;
                                source.Play();
                                return clip;
                        }
                }

                var newSource = new GameObject("new sfx souce").AddComponent<AudioSource>();

                newSource.loop = false;
                newSource.outputAudioMixerGroup = sfxMixerGroup;
                sfxSources.Add(newSource);
                newSource.clip = clip;
                newSource.Play();
                return clip;
        }

        private void PlayNarrator(string soundName)
        {
                var clip = soundDB.GetRandomClipFromSound(soundName);
                if(narratorSource.isPlaying) return;

                narratorSource.clip = clip;
                narratorSource.Play();

        }
        
        public void FadeIn()
        {
                if (!isFading)
                {
                        StartCoroutine(FadeMixerGroupVolume(maxVolume, fadeInDuration));
                }
        }

        public void FadeOut()
        {
                if (!isFading)
                        StartCoroutine(FadeMixerGroupVolume(minVolume, fadeOutDuration));
        }

        private IEnumerator FadeMixerGroupVolume(float targetVolume, float duration)
        {
                isFading = true;
                float currentTime = 0f;
                audioMixer.GetFloat(musicGroup, out float currentVolume);
                Debug.Log($"new {currentVolume} , ");

                currentVolume = Mathf.Pow(10, currentVolume / 20); // Convert from decibels to linear

                float targetLinearVolume = Mathf.Pow(10, targetVolume / 20);
                while (currentTime < duration)
                {
                        currentTime += Time.deltaTime;
                        float newVolume = Mathf.Lerp(currentVolume, targetLinearVolume, currentTime / duration);
                        Debug.Log($"new {currentVolume} vole {targetLinearVolume}, step {currentTime / duration}, ");
                        audioMixer.SetFloat(musicGroup, Mathf.Log10(newVolume) * 20); // Convert back to decibels
                        yield return null;
                }

                audioMixer.SetFloat(musicGroup, targetVolume);
                isFading = false;
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
                                GameManager.Instance.ShowBirdsNotification();
                        }
                        if (GUILayout.Button("Play Thunder Narrator"))
                        {
                                GameManager.Instance.ShowThunderNotification();
                        }
                        if (GUILayout.Button("Play Storm Narrator"))
                        {
                                GameManager.Instance.ShowStormNotification();
                        }                      
                        
                        if (GUILayout.Button("Player one wins"))
                        {
                                GameManager.Instance.PlayerOneWin();
                        }                        
                        if (GUILayout.Button("Player two wins"))
                        {
                                GameManager.Instance.PlayerTwoWin();
                        }

                        if (GUILayout.Button("Add player"))
                        {
                                PlayDing();
                                GameManager.Instance.AddPlayer();
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
                FadeIn();
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


        public void PlayCounter()
        {
                var clip = soundDB.GetRandomClipFromSound("counter");
                PlaySfx(clip);
        }

}
        
        
        
