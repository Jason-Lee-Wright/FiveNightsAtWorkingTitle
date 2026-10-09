using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Event", menuName = "Events/Sound Event")]
public class SoundEvent : ScriptableObject
{
    public Action<AudioClip> OnPlay;


    public void PlaySound(AudioClip clip) => OnPlay?.Invoke(clip);

    /// <summary>
    /// Takes in an array of audio clips and plays a random clip from array
    /// </summary>
    /// <param name="clips"></param>
    public void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip randdomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        OnPlay?.Invoke(randdomClip);
    }
}
