using System.Collections;
using UnityEngine;

public class ClockSystem : MonoBehaviour
{
    private int currentTime;
    private int endTime = 6;
    private int hourLength = 60; // 60 seconds in an in game hour

    [SerializeField] private IntEvent currentTimeEvent;

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

    }
}