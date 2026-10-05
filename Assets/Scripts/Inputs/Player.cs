using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputActions playerInput;
    #region Camera Settings
    [Header("Player Camera Setiing")]
    [SerializeField] float turnSpeed;
    [SerializeField] float maxTurnAngleY;
    [SerializeField] float maxTurnAngleX;
    private float pitchY;
    private float pitchX;
    private Vector2 lookInput;
    #endregion

    private Camera playerHead;

    [Header("Camera Menu Setiing")]
    [SerializeField] private GameObject cameraScreen; // Needs to be a prefab and prefabs needs to match the gameplay prefab position
    [SerializeField] private Vector3 screenOffset; // Where should the player be when going to screen
    [SerializeField] private UnityEvent onCameraMenu;
    [SerializeField] private UnityEvent offCameraMenu;

    [SerializeField] private MoveCameraEvent moveCameraEvent; // If we any interactable that want us to move
    [SerializeField] private float moveDuration; // How long the camera takes to get into position
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool onCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHead = Camera.main;
        oldPosition = playerHead.transform.position;
        oldRotation = playerHead.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeadMovement();
    }

    #region Player Camera Logic 
    private void HandleHeadMovement()
    {
        pitchX += lookInput.x * turnSpeed * Time.deltaTime;
        pitchX = Mathf.Clamp(pitchX, -maxTurnAngleX, maxTurnAngleX);

        pitchY -= lookInput.y * turnSpeed * Time.deltaTime;
        pitchY = Mathf.Clamp(pitchY, -maxTurnAngleY, maxTurnAngleY);


        playerHead.transform.localEulerAngles = new Vector3(pitchY, pitchX, 0);
    }

    public void ResetHead()
    {
        pitchX = 0f;
        pitchY = 0f;
    }

    private void SetLookInput(Vector2 inputValue)
    {
        lookInput = inputValue.normalized;
    }

    #endregion

    #region Camera Menu Logic
    /// <summary>
    /// Handles if the menu can open or not
    /// </summary>
    private void HandleCameraMenu()
    {
        if (isMoving) return;

        if (onCamera)
        {
            offCameraMenu?.Invoke();
            MoveOldPosition();
            onCamera = false;
        }
        else
        {
            onCameraMenu?.Invoke();
            ResetHead();
            SetTargetCamera(cameraScreen.transform.position + screenOffset, cameraScreen.transform.rotation);
            onCamera = true;
        }
    }
    /// <summary>
    /// Starts a coroutine 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetRotation"></param>
    public void SetTargetCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        StartCoroutine(MoveIntoPosiition(targetPosition, targetRotation));
    }
    public void MoveOldPosition()
    {
        StartCoroutine(MoveIntoPosiition(oldPosition, oldRotation));
    }

    private IEnumerator MoveIntoPosiition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isMoving = true;
        Vector3 startPosition = playerHead.transform.position;
        Quaternion startRotation = playerHead.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float timeProgress = elapsedTime / moveDuration;

            playerHead.transform.position = Vector3.Lerp(startPosition, targetPosition, timeProgress);
            playerHead.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeProgress);
            yield return null;
        }
        isMoving = false;
        playerHead.transform.position = targetPosition;
        playerHead.transform.rotation = targetRotation;
    }

    #endregion

    private void OnEnable()
    {
        playerInput.lookEvent += SetLookInput;
        playerInput.onCameraStarted += HandleCameraMenu;
        moveCameraEvent.onEvent += SetTargetCamera;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        playerInput.lookEvent -= SetLookInput;
        playerInput.onCameraStarted -= HandleCameraMenu;
        moveCameraEvent.onEvent -= SetTargetCamera;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        isMoving = false;
        onCamera = false;
        ResetHead();
        if (playerHead == null) return;
        playerHead.transform.position = oldPosition;
        playerHead.transform.rotation = oldRotation;
    }
}
