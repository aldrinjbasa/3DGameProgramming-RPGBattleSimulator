using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] listOfSounds;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        foreach(Sound s in listOfSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.soundClip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string soundName)
    {
        Sound sounds = Array.Find(listOfSounds, sound => sound.name == soundName);
        sounds.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sounds = Array.Find(listOfSounds, item => item.name == soundName);
        if (sounds == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        sounds.source.Stop();
    }
}
