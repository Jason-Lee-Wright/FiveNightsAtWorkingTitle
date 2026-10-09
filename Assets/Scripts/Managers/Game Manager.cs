using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState survivedState; // Player survived the night

    [SerializeField] private GameState currentState;
    [SerializeField] private GameState lastState;

    [SerializeField] private UnityEvent onMainMenu;
    [SerializeField] private UnityEvent onGameplay;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onGameOver;
    [SerializeField] private UnityEvent onSurvived;

    [SerializeField] private PlayerInputActions playerInputActions;
    private static GameManager instance;

    public static GameManager Instance => instance;


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
    #region Game State logic
    public void ChangeGameState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
            lastState = currentState;
        }

        currentState = newState;
        currentState.EnterState();
    }

    public void SwitchToGameplay()
    {
        ChangeGameState(gameplayState);
        onGameplay?.Invoke();
    }

    public void SwitchToMenu()
    {
        ChangeGameState(mainMenuState);
        onMainMenu?.Invoke();
    }

    public void SwitchToPause()
    {
        ChangeGameState(pauseState);
        onPause?.Invoke();
    }

    public void SwitchToGameOver()
    {
        ChangeGameState(gameOverState);
        onGameOver?.Invoke();
    }
    public void SwitchToWinner()
    {
        ChangeGameState(survivedState);
        onSurvived?.Invoke();
    }

    public void HandlePause()
    {
        
        if (currentState == gameplayState)
        {
            Debug.Log("Paused");
            SwitchToPause();
            PauseGame();
        }
        else if (currentState == pauseState)
        {
            SwitchToGameplay();
            ResumeGame();
        }
    }

    #endregion
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void DisableGameplayInputs()
    {
        playerInputActions.DisableInteract();
        playerInputActions.DisableLook();
        playerInputActions.DisableCamera();
        ResumeGame();
    }

    public void EnableGameplayInputs()
    {
        playerInputActions.EnableInteract();
        playerInputActions.EnableLook();
        playerInputActions.EnableCamera();
    }

    private void OnEnable()
    {
        mainMenuState.onEnterState += playerInputActions.EnableInteract;
        mainMenuState.onExitState += playerInputActions.DisableInteract;
        mainMenuState.onEnterState += playerInputActions.DisableLook;

        gameplayState.onEnterState += EnableGameplayInputs;
        gameplayState.onExitState += DisableGameplayInputs;

        gameplayState.onEnterState += playerInputActions.EnablePause;
        gameplayState.onExitState += playerInputActions.DisablePause;

        pauseState.onEnterState += playerInputActions.EnablePause;
        pauseState.onExitState += playerInputActions.DisablePause;


        playerInputActions.onPauseStarted += HandlePause;
    }

    private void OnDisable()
    {
        mainMenuState.onEnterState -= playerInputActions.EnableInteract;
        mainMenuState.onExitState -= playerInputActions.DisableInteract;
        mainMenuState.onEnterState -= playerInputActions.DisableLook;

        gameplayState.onEnterState -= EnableGameplayInputs;
        gameplayState.onExitState -= DisableGameplayInputs;

        gameplayState.onEnterState -= playerInputActions.EnablePause;
        gameplayState.onExitState -= playerInputActions.DisablePause;

        pauseState.onEnterState -= playerInputActions.EnablePause;
        pauseState.onExitState -= playerInputActions.DisablePause;


        playerInputActions.onPauseStarted -= HandlePause;
    }
}
