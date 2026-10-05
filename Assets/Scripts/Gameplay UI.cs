using UnityEngine;
using TMPro;
public class GameplayUI : MonoBehaviour
{
    [SerializeField] GameState gameplayState;
    [SerializeField] private TextMeshProUGUI timeUI;
    [SerializeField] private IntEvent currentTimeEvent;

    [SerializeField] private GameObject camMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTime(int currentTime)
    {
        int displayedTime = currentTime % 12;

        if (displayedTime == 0) displayedTime = 12;

        timeUI.text = $"{displayedTime} AM";
    }

    public void HandleCamMenu()
    {
        camMenu.SetActive(!camMenu.activeInHierarchy);
    }

    public void FalseCamMenu()
    {
        camMenu.SetActive(false);
    }

    public void TrueCamMenu()
    {
        camMenu.SetActive(true);
    }
    private void OnEnable()
    {
        currentTimeEvent.onEvent += SetTime;
        gameplayState.onEnterState += FalseCamMenu;
    }

    private void OnDisable()
    {
        currentTimeEvent.onEvent -= SetTime;
        gameplayState.onEnterState -= FalseCamMenu;
    }
}
