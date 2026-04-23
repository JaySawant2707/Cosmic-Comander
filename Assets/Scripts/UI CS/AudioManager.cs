using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;

    public bool loop;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("SFX Pool")]
    [SerializeField] private int poolSize = 10;

    private List<AudioSource> sfxPool;
    private int poolIndex = 0;

    [Header("Sounds")]
    public Sound[] sounds;
    private Dictionary<string, Sound> soundMap;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    void Awake()
    {
        // Singleton
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

        // Build sound dictionary
        soundMap = new Dictionary<string, Sound>();
        foreach (var s in sounds)
        {
            if (!soundMap.ContainsKey(s.name))
                soundMap.Add(s.name, s);
        }

        // Create SFX pool
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("SFX_" + i);
            go.transform.parent = transform;

            AudioSource source = go.AddComponent<AudioSource>();
            sfxPool.Add(source);
        }
    }

    // 🎯 Get next available source
    private AudioSource GetSource()
    {
        AudioSource source = sfxPool[poolIndex];
        poolIndex = (poolIndex + 1) % sfxPool.Count;
        return source;
    }

    // 🔊 Play SFX (global)
    public void PlaySFX(string name)
    {
        if (!soundMap.ContainsKey(name))
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }

        Sound s = soundMap[name];
        AudioSource source = GetSource();

        source.pitch = s.pitch * Random.Range(0.9f, 1.1f);
        source.PlayOneShot(s.clip, s.volume * sfxVolume * masterVolume);
    }

    // 📍 Play SFX at position (for enemies/explosions)
    public void PlaySFXAtPosition(string name, Vector3 position)
    {
        if (!soundMap.ContainsKey(name)) return;

        Sound s = soundMap[name];

        GameObject temp = new GameObject("TempAudio");
        temp.transform.position = position;

        AudioSource source = temp.AddComponent<AudioSource>();
        source.spatialBlend = 0f; // 2D game (keep 0)
        source.clip = s.clip;
        source.volume = s.volume * sfxVolume * masterVolume;
        source.pitch = s.pitch * Random.Range(0.9f, 1.1f);

        source.Play();
        Destroy(temp, s.clip.length);
    }

    // 🎵 Music
    public void PlayMusic(string name)
    {
        if (!soundMap.ContainsKey(name)) return;

        Sound s = soundMap[name];

        musicSource.clip = s.clip;
        musicSource.volume = s.volume * musicVolume * masterVolume;
        musicSource.loop = s.loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // 🔇 Volume controls
    public void SetMasterVolume(float value)
    {
        masterVolume = value;

        // Re-apply music volume immediately
        if (musicSource != null)
        {
            musicSource.volume = musicVolume * masterVolume;
        }

        // Optional: update all active SFX sources (important!)
        foreach (var source in sfxPool)
        {
            source.volume = sfxVolume * masterVolume;
        }
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume * masterVolume;
    }
}