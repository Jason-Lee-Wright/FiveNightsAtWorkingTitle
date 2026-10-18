using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClockSystem : MonoBehaviour
{
    private int currentTime;
    [SerializeField] private int endTime = 6;
    [SerializeField] private int hourLength = 60; // 60 seconds in an in game hour

    [SerializeField] private IntEvent currentTimeEvent;
    [SerializeField] private UnityEvent onFinishedNight;

    private void Start()
    {
        currentTime = 0; //0 is 12am
        currentTimeEvent.RaiseEvent(currentTime);
        StartCoroutine(BeginClockSystem());
    }

    IEnumerator BeginClockSystem()
    {
        while(currentTime < endTime)
        {
            yield return new WaitForSeconds(hourLength);
            currentTime++;
            currentTimeEvent.RaiseEvent(currentTime);
        }

        onFinishedNight?.Invoke();
        GameManager.Instance.SwitchToWinner();

    }
}