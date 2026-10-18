using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState survivedState; // Player survived the night
    [SerializeField] private GameState optionState;

    [SerializeField] private GameState currentState;
    [SerializeField] private GameState lastState;

    [SerializeField] private UnityEvent onMainMenu;
    [SerializeField] private UnityEvent onGameplay;
    [SerializeField] private UnityEvent onPause;
    [SerializeField] private UnityEvent onGameOver;
    [SerializeField] private UnityEvent onSurvived;
    [SerializeField] private UnityEvent onOption;
    private UnityEvent lastStateEvent;

    [SerializeField] private PlayerInputActions playerInputActions;

    private bool isPaused;
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
            instance.currentState.ExitState();
            instance.lastState = currentState;
        }
        
        instance.currentState = newState;
        instance.currentState.EnterState();
    }

    public void SwitchToGameplay()
    {
        ChangeGameState(gameplayState);
        instance.lastStateEvent = onGameplay;
        instance.onGameplay?.Invoke();
    }

    public void SwitchToMenu()
    {
        ChangeGameState(mainMenuState);
        lastStateEvent = onMainMenu;
        instance.onMainMenu?.Invoke();
    }

    public void SwitchToPause()
    {
        ChangeGameState(pauseState);
        instance.lastStateEvent = onPause;
        instance.onPause?.Invoke();
    }

    public void SwitchToGameOver()
    {
        ChangeGameState(gameOverState);
        instance.lastStateEvent = onGameOver;
        instance.onGameOver?.Invoke();
    }
    public void SwitchToWinner()
    {
        ChangeGameState(survivedState);
        instance.lastStateEvent = onSurvived;
        instance.onSurvived?.Invoke();
    }

    public void SwitchToOption()
    {
        ChangeGameState(optionState);
        instance.lastStateEvent = onOption;
        instance.onOption?.Invoke();
    }

    public void SwitchToLastState()
    {
        ChangeGameState(lastState);
        instance.lastStateEvent?.Invoke();
    }

    public void HandlePause()
    {
        
        if (currentState == gameplayState)
        {
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

    public void MuteOnPause()
    {
        AudioListener.pause = true;
    }

    public void UnmuteOnPause()
    {
        AudioListener.pause = false;
    }


    public void DisableGameplayInputs()
    {
        playerInputActions.DisableInteract();
        playerInputActions.DisableLook();
        playerInputActions.DisableCamera();
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableGameplayInputs()
    {
        playerInputActions.EnableInteract();
        playerInputActions.EnableLook();
        playerInputActions.EnableCamera();
        Cursor.lockState = CursorLockMode.Confined;
        ResumeGame();
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
