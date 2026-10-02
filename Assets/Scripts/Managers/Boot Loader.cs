using UnityEngine;
using UnityEngine.SceneManagement;

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
        else
        {
            Debug.Log("Level Manager take us to menu");
        }
        
    }

}

public class BootLoader : MonoBehaviour
{
    private static BootLoader instance;

    public static BootLoader Instance => instance;


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

}
