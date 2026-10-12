using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume;
    [Range(0f, 1f)]
    [SerializeField] private float soundEffectsVolume;
    [Range(0f, 1f)]
    [SerializeField] private float voiceVolume;
    [Range(0f, 1f)]
    [SerializeField] private float backgroundVolume;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private SoundEvent soundEffectEvent;
    [SerializeField] private SoundEvent voiceEffectEvent;
    [SerializeField] private SoundEvent backgroundEvent;

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

    public void PlaySoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,masterVolume * soundEffectsVolume);
        
    }

    public void PlayVoice(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,masterVolume * voiceVolume);
    }

    public void PlayBackgroundAudio(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.volume = masterVolume * backgroundVolume;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsVolume = volume;
    }

    public void SetVoiceVolume(float volume)
    {
        voiceVolume = volume;
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = volume;
    }

    public void StopBackgroundAudio()
    {
        audioSource.Stop();
    }

    private void OnEnable()
    {
        soundEffectEvent.OnPlay += PlaySoundEffect;
        voiceEffectEvent.OnPlay += PlayVoice;
        backgroundEvent.OnPlay += PlayBackgroundAudio;
    }

    private void OnDisable()
    {
        soundEffectEvent.OnPlay -= PlaySoundEffect;
        voiceEffectEvent.OnPlay -= PlayVoice;
        backgroundEvent.OnPlay -= PlayBackgroundAudio;
    }
}