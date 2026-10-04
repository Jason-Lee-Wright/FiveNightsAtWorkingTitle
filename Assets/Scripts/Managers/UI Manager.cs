using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform canvasUI;
    [Header("Game states")]
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState survivedState; // Player survived the night

    [SerializeField] private UnityEvent enableMainMenu;
    [SerializeField] private UnityEvent enableGameplay;
    [SerializeField] private UnityEvent enablePause;
    [SerializeField] private UnityEvent enablegameOverState;
    [SerializeField] private UnityEvent enableSurvived;

    private void Awake()
    {
        // If null will get first child in UI Manager
        canvasUI ??= transform.GetChild(0);
    }

    public void EnableMainMenu()
    {
        DisableAllMenus();
        enableMainMenu?.Invoke();
    }

    public void EnableGameplay()
    {
        DisableAllMenus();
        enableGameplay?.Invoke();
    }

    public void EnablePauseMenu()
    {
        DisableAllMenus();
        enablePause?.Invoke();
    }

    public void EnableGameOverMenu()
    {
        DisableAllMenus();
        enablegameOverState?.Invoke();
    }
    public void EnableSurvivedMenu()
    {
        DisableAllMenus();
        enableSurvived?.Invoke();
    }

    public void DisableAllMenus()
    {
        canvasUI ??= transform.GetChild(0);

        foreach (Transform UI in canvasUI)
        {
            UI.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        mainMenuState.onEnterState += EnableMainMenu;
        gameplayState.onEnterState += EnableGameplay;
        pauseState.onEnterState += EnablePauseMenu;
        gameOverState.onEnterState += EnableGameOverMenu;
        survivedState.onEnterState += EnableSurvivedMenu;

    }

    private void OnDisable()
    {
        mainMenuState.onEnterState -= EnableMainMenu;
        gameplayState.onEnterState -= EnableGameplay;
        pauseState.onEnterState -= EnablePauseMenu;
        gameOverState.onEnterState -= EnableGameOverMenu;
        survivedState.onEnterState -= EnableSurvivedMenu;
    }
}
