using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public static class BootLoad
{
    const string bootSceneName = "Boot Loader";
    // Loads before scene is loadeded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        // When not in boot loader scene add boot loader additive 
        if (activeScene != bootSceneName)
        {
            SceneManager.LoadScene(bootSceneName, LoadSceneMode.Additive);
        }
        // When in boot loader scene, scene will switch to main menu from Level Manager
    }

}

public class BootLoader : MonoBehaviour
{
    private static BootLoader instance;

    public static BootLoader Instance => instance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
