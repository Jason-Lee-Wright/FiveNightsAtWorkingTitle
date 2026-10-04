using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private PlayerInputActions inputActions;

    [SerializeField] private Vector2 cursorPosition;
    private Camera mainCamera;
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
                interactable.OnInteract();
            }
        }
    }

    private void OnEnable()
    {
        inputActions.pointerEvent += SetCursorPosition;
        inputActions.onInteractStarted += Interact;
    }

    private void OnDisable()
    {
        inputActions.pointerEvent -= SetCursorPosition;
        inputActions.onInteractStarted -= Interact;
    }
}
