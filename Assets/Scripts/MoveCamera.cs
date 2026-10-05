using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float moveDuration;
    [SerializeField] private float moveSpeed;
    private Camera mainCamera;
    private Vector3 cameraOldPosition;
    private Vector3 targetPosition;
    [SerializeField] private VoidEvent CameraEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;

        cameraOldPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        //if (Vector3.Distance())
    }

    public void HandleCameraPosition()
    {

        if (Vector3.Distance(mainCamera.transform.position, transform.position) < .1f)
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
        Debug.Log("Moving camera to me");
        mainCamera.transform.position = transform.position;
        //StartCoroutine(MoveIntoPosiition(transform.position));
    }

    public void MoveOldPosition()
    {
        Debug.Log("Moving camera to old position");
        mainCamera.transform.position = cameraOldPosition;
        //StartCoroutine(MoveIntoPosiition(cameraOldPosition));
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
