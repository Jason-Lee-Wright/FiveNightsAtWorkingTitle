using UnityEngine;
using UnityEngine.Events;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private Vector3 cameraOldPosition;
    [SerializeField] private VoidEvent CameraEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;

        cameraOldPosition = mainCamera.transform.position;
    }

    public void HandleCameraPosition()
    {
        if (mainCamera.transform.position == transform.position)
        {
            MoveOldPosition();
        }
        else
        {
            MoveToMe();
        }
    }

    public void MoveToMe()
    {
        mainCamera.transform.position = transform.position;
    }

    public void MoveOldPosition()
    {
        mainCamera.transform.position = cameraOldPosition;
    }

    private void OnEnable()
    {
        CameraEvent.onEvent += HandleCameraPosition; 
    }

    private void OnDisable()
    {
        CameraEvent.onEvent -= HandleCameraPosition;
    }

}
