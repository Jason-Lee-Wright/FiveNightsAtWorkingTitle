using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Event", menuName = "Events/Sound Event")]
public class SoundEvent : ScriptableObject
{
    public Action<AudioClip> OnPlay;

    public void PlaySound(AudioClip clip) => OnPlay?.Invoke(clip);
}
