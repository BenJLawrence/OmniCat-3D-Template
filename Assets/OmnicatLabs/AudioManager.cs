using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace OmnicatLabs.Managers
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public bool atPosition;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(.1f, 3f)]
        public float pitch = 1f;
        [Range(0f, 1f)]
        public float spatialBlend = 0f;
        [HideInInspector]
        public AudioSource source;
        public bool loop;
        public bool playOnAwake;
        public AudioMixerGroup output;
        public AudioReverbPreset reverbPreset;
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        public List<Sound> sounds;
        public AudioMixer mixer;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            foreach (Sound sound in sounds)
            {
                if (!sound.atPosition)
                    AssignSource(gameObject, sound);
            }
        }

        public void Play(string name)
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }
            AssignSource(gameObject, sound);
            sound.source.Play();
        }

        public void Play(string name, Vector3 position)
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            GameObject effect = Instantiate(Resources.Load("Prefabs/SoundEffect") as GameObject, position, Quaternion.identity);
            AssignSource(effect, sound);
            sound.source.Play();
        }

        public void Play(string name, GameObject go)
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }
            AssignSource(go, sound);
            sound.source.Play();
        }

        //***NOT READY YET***
        //public void PlayFadeIn(string name, Vector3 position)
        //{
        //    Sound sound = sounds.Find(s => s.name == name);
        //    if (sound == null)
        //    {
        //        Debug.LogWarning("Sound: " + name + " not found");
        //        return;
        //    }

        //    GameObject effect = Instantiate(Resources.Load("Prefabs/SoundEffect") as GameObject, position, Quaternion.identity);
        //    AssignSource(effect, sound);
        //    StartCoroutine(FadeMixerGroup.StartFade(mixer, "Volume", .5f, 1));
        //}

        //***NOT READY YET***
        //public void PlayFadeIn(string name)
        //{
        //    Sound sound = sounds.Find(s => s.name == name);
        //    if (sound == null)
        //    {
        //        Debug.LogWarning("Sound: " + name + " not found");
        //        return;
        //    }

        //    AssignSource(gameObject, sound);
        //    //sound.source.Play();
        //    StartCoroutine(FadeMixerGroup.StartFade(mixer, "Volume", .5f, sound.volume));
        //}

        public void Stop(string name)
        {
            Sound sound = sounds.Find(s => s.name == name);
            if (sound == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }
            sound.source.Stop();
        }

        private void AssignSource(GameObject obj, Sound sound)
        {
            sound.source = obj.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = sound.output;
            if (!obj.GetComponent<AudioReverbFilter>())
                obj.AddComponent<AudioReverbFilter>().reverbPreset = sound.reverbPreset;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }
}
