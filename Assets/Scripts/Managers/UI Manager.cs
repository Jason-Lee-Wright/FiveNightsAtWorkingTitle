using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float warningTime;
    [SerializeField] private Transform canvasUI;
    [Header("Game states")]
    [SerializeField] private GameState mainMenuState;
    [SerializeField] private GameState gameplayState;
    [SerializeField] private GameState pauseState;
    [SerializeField] private GameState gameOverState;
    [SerializeField] private GameState survivedState; // Player survived the night
    [SerializeField] private GameState optionState;

    [SerializeField] private UnityEvent enableMainMenu;
    [SerializeField] private UnityEvent enableGameplay;
    [SerializeField] private UnityEvent enablePause;
    [SerializeField] private UnityEvent enablegameOverState;
    [SerializeField] private UnityEvent enableSurvived;
    [SerializeField] private UnityEvent enableOption;

    [SerializeField] private UnityEvent onWarningStart;
    [SerializeField] private UnityEvent onWarningEnd;

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

    public void EnableOptionMenu()
    {
        DisableAllMenus();
        enableOption?.Invoke();
    }

    public void DisableAllMenus()
    {
        canvasUI ??= transform.GetChild(0);

        foreach (Transform UI in canvasUI)
        {
            UI.gameObject.SetActive(false);
        }
    }

    IEnumerator WarningScreen()
    {
        onWarningStart?.Invoke();
        yield return new WaitForSeconds(warningTime);
        onWarningEnd?.Invoke();
    }

    public void StartWarning()
    {
        StartCoroutine(WarningScreen());
    }

    public void EndWarning()
    {
        onWarningEnd?.Invoke();
    }

    private void OnEnable()
    {
        mainMenuState.onEnterState += EnableMainMenu;
        mainMenuState.onExitState += EndWarning;
        gameplayState.onEnterState += EnableGameplay;
        pauseState.onEnterState += EnablePauseMenu;
        gameOverState.onEnterState += EnableGameOverMenu;
        survivedState.onEnterState += EnableSurvivedMenu;
        optionState.onEnterState += EnableOptionMenu;

    }

    private void OnDisable()
    {
        mainMenuState.onEnterState -= EnableMainMenu;
        mainMenuState.onExitState -= EndWarning;
        gameplayState.onEnterState -= EnableGameplay;
        pauseState.onEnterState -= EnablePauseMenu;
        gameOverState.onEnterState -= EnableGameOverMenu;
        survivedState.onEnterState -= EnableSurvivedMenu;
        optionState.onEnterState -= EnableOptionMenu;
    }
}
