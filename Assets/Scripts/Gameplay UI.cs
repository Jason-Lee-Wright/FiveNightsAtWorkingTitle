using UnityEngine;
using TMPro;
public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeUI;
    [SerializeField] private IntEvent currentTimeEvent;
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

    private void OnEnable()
    {
        currentTimeEvent.onEvent += SetTime;
    }

    private void OnDisable()
    {
        currentTimeEvent.onEvent -= SetTime;
    }
}
