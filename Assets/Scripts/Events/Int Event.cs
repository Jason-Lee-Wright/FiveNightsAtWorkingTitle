using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Int Event", menuName = "Events/Int Event")]
public class IntEvent : ScriptableObject
{
    /// <summary>
    /// Sub to action example : intEvent.onEvent += INeedInt
    /// </summary>
    public Action<int> onEvent;

    /// <summary>
    /// Call when u want to send the int (time) out to people subbed example : intEvent.RaiseEvent(currentTime)
    /// </summary>
    /// <param name="Int"></param>
    public void RaiseEvent(int Int) => onEvent?.Invoke(Int);
}