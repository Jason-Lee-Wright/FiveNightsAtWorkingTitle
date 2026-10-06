using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float masterVolume;
    [SerializeField] private float soundEffectsVolume;
    [SerializeField] private float voiceVolume;

    private SoundManager instance;

    private SoundManager Instace => instance;

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
}
