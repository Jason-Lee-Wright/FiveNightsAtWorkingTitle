using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Move Camera Event", menuName = "Events/Move Camera Event")]
public class MoveCameraEvent : ScriptableObject
{
    public Action<Vector3, Quaternion> onEvent;

    public Action onEventCanceled;
    public void MoveEvent(Vector3 vector3, Quaternion quaternion) => onEvent?.Invoke(vector3,quaternion);

    public void CanceledEvent() => onEventCanceled?.Invoke();
}
