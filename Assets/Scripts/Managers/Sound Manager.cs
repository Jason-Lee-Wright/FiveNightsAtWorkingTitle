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
    private AudioSource audioSource;

    [SerializeField] private SoundEvent soundEffectEvent;
    [SerializeField] private SoundEvent voiceEffectEvent;

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

    private void OnEnable()
    {
        soundEffectEvent.OnPlay += PlaySoundEffect;
        voiceEffectEvent.OnPlay += PlayVoice;
    }

    private void OnDisable()
    {
        soundEffectEvent.OnPlay -= PlaySoundEffect;
        voiceEffectEvent.OnPlay -= PlayVoice;
    }
}