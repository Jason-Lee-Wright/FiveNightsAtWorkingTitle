using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private float masterVolume;
    [SerializeField] private float soundEffectsVolume;
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
        audioSource.PlayOneShot(clip,1);
        
    }

    public void PlayVoice(AudioClip clip)
    {
        audioSource.PlayOneShot(clip,1);
    }

    private void OnEnable()
    {
        soundEffectEvent.OnPlay += PlaySoundEffect;
    }

    private void OnDisable()
    {
        soundEffectEvent.OnPlay -= PlaySoundEffect;
    }
}