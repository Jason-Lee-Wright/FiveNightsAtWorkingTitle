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

        if (activeScene != bootSceneName)
        {
            SceneManager.LoadScene(bootSceneName, LoadSceneMode.Additive);
        }
        else // Same scene as boot loader is handled in level manager 
        {
            //LevelManager.Instance.LoadMainMenu();
            //SceneManager.LoadScene(bootSceneName, LoadSceneMode.Additive);
        }
        
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
