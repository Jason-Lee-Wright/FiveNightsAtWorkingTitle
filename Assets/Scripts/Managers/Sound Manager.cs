using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float sfxSound;

    private SoundManager instance;

    private SoundManager Instace => instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
