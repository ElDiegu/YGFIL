using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace YGFIL.Managers
{
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeSounds();
        //UpdateMusic(PlayerPrefs.GetFloat("music", 0.5f));
        //UpdateVolume(PlayerPrefs.GetFloat("noise", 0.5f));
    }

    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //InitializeSounds();
    }

    private void InitializeSounds()
    {
        foreach (Sound s in sounds)
        {
            if (s.clip == null)
            {
                Debug.LogError($"Sound {s.name} has no audio clip assigned!");
                continue;
            }

            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        if (s.source == null)
        {
            Debug.LogError($"AudioSource for sound {name} is null!");
            return;
        }
        s.source.Play();
    }
    public void PlayRandomPitch(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        s.source.pitch = s.pitch;
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Pause();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public void UpdateVolume(float v)
    {
        foreach(Sound s in sounds)
        {
            if (!s.music) {
                s.volume = v;
                s.source.volume = s.volume;
            }
        }
    }

    public void UpdateMusic(float v)
    {
        foreach (Sound s in sounds)
        {
            if (s.music)
            {
                s.volume = v;
                s.source.volume = s.volume;
            }
        }
    }

    public void DecreaseMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.music)
            {
                s.source.volume = s.source.volume * 0.5f;
            }
        }
    }

    public void IncreaseMusic()
    {
        foreach (Sound s in sounds)
        {
            if (s.music)
            {
                s.source.volume = s.source.volume * 2f;
            }
        }
    }

    public void StopLoops()
    {
        foreach (Sound s in sounds)
        {
            if (s.loop & !s.music)
            {
                s.source.volume = s.source.volume * 0.01f;
            }
        }
    }

    public void ResumeLoops()
    {
        foreach (Sound s in sounds)
        {
            if (s.loop & !s.music)
            {
                s.source.volume = s.source.volume * 100f;
            }
        }
    }
}
}