using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsManager : MonoBehaviour
{
    private static OptionsManager __inst__;
    public static OptionsManager Instance
    {
        get => __inst__;
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

    [SerializeField] private AudioMixer audioMixer;

    public static Options currentOptions;
    public class Options
    {
        public float sensitivity = 1f;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        // Load settings if needed.

    }

    public void EditMixerVariable(string name, float value)
    {
        // A 100 is sent
        audioMixer.SetFloat(name, Linear2Decibels(value));
    }

    public float GetMixerVariable(string name)
    {
        float value = 0f; audioMixer.GetFloat(name, out value); return Decibels2Linear(value);
    }

    public float Linear2Decibels(float linear)
    { 
        return linear != 0 ? 20.0f * Mathf.Log10(linear) : -144.0f;
    }
    public float Decibels2Linear(float dB)
    {
        return Mathf.Pow(10.0f, dB / 20.0f);
    }

}
