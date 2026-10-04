using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "Player Actions", menuName = "Input Action Reader/Player Actions")]
public class PlayerInputActions : ScriptableObject, InputActions.IPlayerActions
{
    private InputActions inputActions;

    public event UnityAction<Vector2> lookEvent;
    private bool canLook;

    public event UnityAction<Vector2> pointerEvent;

    public event UnityAction onInteractStarted;
    public event UnityAction onInteractPerformed;
    private bool canInteract;

    public event UnityAction onCameraStarted;
    public event UnityAction onCameraPerformed;
    private bool canCamera;

    public event UnityAction onPauseStarted;
    private bool canPause;
    /// <summary>
    /// Mouse look value
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!canLook) return;

        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }
    /// <summary>
    /// True allow look inputs, False disable look inputs
    /// </summary>
    /// <param name="newBool"></param>
    public void DisableLook()
    {
        canLook = false;
    }
    public void EnableLook()
    {
        canLook = true;
    }
    /// <summary>
    /// Mouse position
    /// </summary>
    /// <param name="context"></param>
    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        pointerEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;

        if (context.started) onInteractStarted?.Invoke();

        if (context.performed) onInteractPerformed?.Invoke();
    }
    /// <summary>
    /// True allows interact, false disables interact inputs 
    /// </summary>
    /// <param name="newBool"></param>
    public void DisableInteract()
    {
        canInteract = false;
    }
    public void EnableInteract()
    {
        canInteract = true;
    }

    public void OnCameraMenu(InputAction.CallbackContext context)
    {
        if (!canCamera) return;

        if (context.started) onCameraStarted?.Invoke();

        if (context.performed) onCameraPerformed?.Invoke();
    }
    /// <summary>
    /// True allows camera menu, false will disable this
    /// </summary>
    /// <param name="newBool"></param>
    public void DisableCamera()
    {
        canCamera = false;
    }
    public void EnableCamera()
    {
        canCamera = true;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!canPause) return;

        if (context.started) onPauseStarted?.Invoke();
    }
    /// <summary>
    /// True allows pause, false will disable pause input
    /// </summary>
    /// <param name="newBool"></param>
    public void DisablePause()
    {
        canPause = false;
    }
    public void EnablePause()
    {
        canPause = true;
    }

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Player.SetCallbacks(this);
        inputActions.Player.Enable();
    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    
}
