using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class CameraInteractable : Interactable, ICancelable
{
    [SerializeField] MoveCameraEvent moveCameraEvent;

    [SerializeField] private UnityEvent onCancel;

    /// <summary>
    /// I thought we could use this for doors if we have the players move to them
    /// </summary>
    public override void OnInteract()
    {
        base.OnInteract();
        moveCameraEvent.MoveEvent(transform.position,transform.rotation);
    }
    public virtual void Cancel()
    {
        onCancel?.Invoke();
        moveCameraEvent.CanceledEvent();
    }
}
