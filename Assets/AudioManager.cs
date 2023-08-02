using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager __inst__;
    public static AudioManager Instance
    {
        get
        {
            if (__inst__ == null)
            {
                __inst__ = new GameObject("[AUDIO MANAGER]").AddComponent<AudioManager>();
            }
  
            return __inst__;
        }
        set
        {
            if (__inst__ != null)
            {
                Destroy(value);
                return;
            }
            __inst__ = value;
        }
    }


    public List<AudioClip> availableClips = new List<AudioClip>();
    private GameObject rootForSounds;
    // We use this to play sounds.
    List<AudioSource> availableAudiosources = new List<AudioSource>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        for (int i = 0; i<3; i++) // Spawn audiosources
        {
            GenerateNewAudioSource();
        }
    }

    private void CreateRoot()
    {
        if (rootForSounds != null) return;
        rootForSounds = new GameObject("[AUDIOMANAGER] - AudioSource Root");
    }

    private void ConfigureAS(AudioSource aSource, float volume = 0.3f, int priority = 120)
    {
        availableAudiosources.Add(aSource);
        aSource.playOnAwake = false;
        aSource.spatialBlend = 0;
        aSource.volume = volume;
        aSource.priority = priority;
    }

    private AudioSource FindUsableAS()
    {
        AudioSource audioS;
        if (availableAudiosources.Count <= 0) audioS = GenerateNewAudioSource();
        audioS = availableAudiosources.Find(x => x.isPlaying == false);
        if (audioS == null) audioS = GenerateNewAudioSource();
        return audioS;
    }

    private AudioSource GenerateNewAudioSource()
    {
        var a = new GameObject("Controlled Audiosource");
        CreateRoot();
        a.transform.parent = rootForSounds.transform;
        var b = a.AddComponent<AudioSource>();
        ConfigureAS(b);
        return b;
    }

    public void PlaySound(AudioClip clip, float volume = 0.5f)
    {
        AudioSource reference = FindUsableAS();
        reference.clip = clip;
        reference.volume = volume;
        reference.Play();
    }

    public void PlaySound(AudioClip[] clip, float volume = 0.5f)
    {
        AudioSource reference = FindUsableAS();
        reference.clip = clip[Random.Range(0, clip.Length)];
        reference.volume = volume;
        reference.Play();
    }

    public void PlaySound(string clip, float volume = 0.5f)
    {
        AudioSource reference = FindUsableAS();
        var clipp = availableClips.Find(x => x.name == clip);
        if (clipp == null)
        {
            Debug.LogWarning("Clip " + clip + " not found!"); 
            return;
        }
        reference.clip = clipp;
        reference.volume = volume;
        reference.Play();
    }
}
