using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Void Event", menuName = "Events/Void Event")]
public class VoidEvent : ScriptableObject
{
    /// <summary>
    /// Sub to event to call that method. Example : thisEvent.onEvent += Explode
    /// </summary>
    public Action onEvent;

    /// <summary>
    /// Call this to raise the event so methods can be done. Example : thisEvent.RaiseEvent();, The thing will then explode when subbed
    /// </summary>
    public void RaiseEvent() => onEvent?.Invoke();
}
