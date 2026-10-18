using UnityEngine;
using System;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSource;

    private static SoundManager instance;

    public static SoundManager Instace => instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayBackgroundAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Sound mixer values are diffrent and not from 0-1 
    /// </summary>
    /// <param name="soundString"></param>
    /// <param name="volume"></param>
    private void SetMixerVolume(string soundString, float volume) // Converts volume into decibles
    {
        float decibels;

        if (volume > 0.0001f)
        {
            decibels = Mathf.Log10(volume) * 20f;
        }
        else
        {
            decibels = -80f;
        }

        instance.audioMixer.SetFloat(soundString, decibels);
    }

    public void SetMasterVolume(float volume)
    {
        SetMixerVolume("Master", volume);
    }

    public void SetSoundEffectsVolume(float volume)
    {
        SetMixerVolume("SFX", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        SetMixerVolume("Voice", volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        SetMixerVolume("Background", volume);
    }

    public void SetScareVolume(float volume)
    {
        SetMixerVolume("Scare", volume);
    }

    public void StopBackgroundAudio()
    {
        audioSource.Stop();
    }

    public void PauseBackgroundAudio()
    {
        audioSource.Pause();
    }

    public void UnpauseBackgroundAudio()
    {

    }

    public void Pause()
    {
        AudioListener.pause = true;
    }

    public void PlayOnPause()
    {
        AudioListener.pause = false;
    }
}