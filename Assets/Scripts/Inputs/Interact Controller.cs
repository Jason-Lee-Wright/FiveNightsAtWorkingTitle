using UnityEngine;
using UnityEngine.Events;

public class InteractController : MonoBehaviour
{
    [SerializeField] private PlayerInputActions inputActions;
    [SerializeField] private UnityEvent onInteractionCanceled;
    private Vector2 cursorPosition;
    private Camera mainCamera;

    private IInteractable interact;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCursorPosition(Vector2 mousePosition)
    {
        cursorPosition = mousePosition;
    }

    public void Interact()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(cursorPosition), out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interact = interactable;
                interact.OnInteract();
            }
        }
    }

    public void CancelInteract()
    {
        if (interact == null) return;
        
        if (interact is ICancelable cancelable)
        {
            onInteractionCanceled?.Invoke();
            cancelable.Cancel();
            interact = null;
        }
    }

    private void OnEnable()
    {
        inputActions.pointerEvent += SetCursorPosition;
        inputActions.onInteractStarted += Interact;
        inputActions.onInteractCanceled += CancelInteract;
    }

    private void OnDisable()
    {
        inputActions.pointerEvent -= SetCursorPosition;
        inputActions.onInteractStarted -= Interact;
        inputActions.onInteractCanceled -= CancelInteract;
    }
}
