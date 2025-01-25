using UnityEngine;

namespace Scenes
{
    [CreateAssetMenu(fileName = "SoundDatabase", menuName = "SoundDB", order = 0)]
    public class SoundDatabase : ScriptableObject
    {
        [SerializeField] private AudioClip defaultClip;
        [SerializeField] private Sound[] sounds;

        public AudioClip GetRandomClipFromSound(string soundName)
        {
            var sound = Find(soundName);
            var notFound = sound == null;
            return notFound ? defaultClip : sound.PickRandom();
        }

        public AudioClip[] GetSoundList(string soundName)
        {
            var sound = Find(soundName);
            return sound.Clips;
        }

        public AudioClip GetClipListFromSound(string soundName, int index)
        {
            var sound = Find(soundName);
            var notFound = sound == null;
            return notFound ? defaultClip : sound.Pick(index);
        }
        public Sound Find(string soundName)
        {
            foreach (var sound in sounds)
            {
                if (sound.IsName(soundName))
                {
                    return sound;
                }
            }
            Debug.Log($"Sound with name: {soundName} not found");

            return null;
        }
    }
}