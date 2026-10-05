using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputActions playerInput;

    [SerializeField] private UnityEvent onCameraMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CameraMenu()
    {
        onCameraMenu?.Invoke();
    }

    private void OnEnable()
    {
        playerInput.onCameraStarted += CameraMenu;
    }

    private void OnDisable()
    {
        playerInput.onCameraStarted -= CameraMenu;
    }
}
