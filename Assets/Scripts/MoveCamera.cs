using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float moveDuration;
    private Camera mainCamera;
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
        if (Vector3.Distance(mainCamera.transform.position, transform.position) < .1f)
        {
            MoveOldPosition();
        }
        else
        {
            MoveToMe();
        }
    }
    /// <summary>
    /// Moves camera to the position
    /// </summary>
    public void MoveToMe()
    {
        StartCoroutine(MoveIntoPosiition(transform.position));
    }
    /// <summary>
    /// moves the camera back to old position
    /// </summary>
    public void MoveOldPosition()
    {
        StartCoroutine(MoveIntoPosiition(cameraOldPosition));
    }

    private IEnumerator MoveIntoPosiition(Vector3 targetPosition)
    {
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float timeProgress = elapsedTime / moveDuration;

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, timeProgress);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
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
