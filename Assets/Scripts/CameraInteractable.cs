using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraInteractable : Interactable
{
    [SerializeField] MoveCameraEvent moveCameraEvent;

    /// <summary>
    /// I thought we could use this for doors if we have the players move to them
    /// </summary>
    public override void OnInteract()
    {
        base.OnInteract();
        moveCameraEvent.MoveEvent(transform.position,transform.rotation);
    }
}
