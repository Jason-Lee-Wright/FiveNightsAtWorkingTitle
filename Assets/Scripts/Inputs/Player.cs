using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputActions playerInput;
    [SerializeField] private GetBoolEvent getOnCamEvent;
    #region Camera Settings
    [Header("Player Camera Setiing")]
    [SerializeField] float turnSpeed;
    [SerializeField] float maxTurnAngleY;
    [SerializeField] float maxTurnAngleX;
    private float pitchY;
    private float pitchX;
    private float basePitchY;
    private float basePitchX;
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
    private bool isMoving;
    private bool onCamera;
    private Coroutine cameraMovement;
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
        if (onCamera || isMoving) return;
        pitchX += lookInput.x * turnSpeed * Time.deltaTime;
        pitchX = Mathf.Clamp(pitchX,basePitchX - maxTurnAngleX,basePitchX + maxTurnAngleX);

        pitchY -= lookInput.y * turnSpeed * Time.deltaTime;
        pitchY = Mathf.Clamp(pitchY,basePitchY - maxTurnAngleY,basePitchY + maxTurnAngleY);


        playerHead.transform.localEulerAngles = new Vector3(pitchY, pitchX, 0);
    }

    public void ResetHead()
    {
        pitchX = 0f;
        pitchY = 0f;
        basePitchX = 0f;
        basePitchY = 0f;
        lookInput = Vector2.zero;
    }

    private void SetLookInput(Vector2 inputValue)
    {
        lookInput = inputValue.normalized;
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
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
            QuitCameraMenu();
        }
        else
        {
            StartCameraMenu();
        }
    }

    public void QuitCameraMenu()
    {
        offCameraMenu?.Invoke();
        MoveOldPosition();
        onCamera = false;
    }

    public void StartCameraMenu()
    {
        onCameraMenu?.Invoke();
        ResetHead();
        SetTargetCamera(cameraScreen.transform.position + screenOffset, cameraScreen.transform.rotation);
        onCamera = true;
    }

    private bool GetOnCam() => onCamera;

    /// <summary>
    /// Starts a coroutine 
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="targetRotation"></param>
    public void SetTargetCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (cameraMovement != null )
        {
            StopCoroutine(cameraMovement);
        }

        cameraMovement = StartCoroutine(MoveIntoPosiition(targetPosition, targetRotation));
    }
    public void MoveOldPosition()
    {
        if (cameraMovement != null)
        {
            StopCoroutine(cameraMovement);
        }

        basePitchX = 0f;
        basePitchY = 0f;

        cameraMovement = StartCoroutine(MoveIntoPosiition(oldPosition, oldRotation));
    }

    private IEnumerator MoveIntoPosiition(Vector3 targetPosition, Quaternion targetRotation)
    {
        ResetHead();
        isMoving = true;
        Vector3 startPosition = playerHead.transform.position;
        Quaternion startRotation = playerHead.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration && Vector3.Distance(playerHead.transform.position,targetPosition) > .1f)
        {
            elapsedTime += Time.deltaTime;
            float timeProgress = elapsedTime / moveDuration;

            playerHead.transform.position = Vector3.Lerp(startPosition, targetPosition, timeProgress);
            playerHead.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, timeProgress);
            yield return null;
        }
        playerHead.transform.position = targetPosition;
        playerHead.transform.rotation = targetRotation;

        basePitchY = NormalizeAngle(playerHead.transform.localEulerAngles.x);
        basePitchX = NormalizeAngle(playerHead.transform.localEulerAngles.y);

        pitchY = basePitchY;
        pitchX = basePitchX;

        isMoving = false;
        cameraMovement = null;
    }

    #endregion

    private void OnEnable()
    {
        playerInput.lookEvent += SetLookInput;
        playerInput.onCameraStarted += HandleCameraMenu;
        moveCameraEvent.onEvent += SetTargetCamera;
        moveCameraEvent.onEventCanceled += MoveOldPosition;

        getOnCamEvent.onGetBool += GetOnCam;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        playerInput.lookEvent -= SetLookInput;
        playerInput.onCameraStarted -= HandleCameraMenu;
        moveCameraEvent.onEvent -= SetTargetCamera;
        moveCameraEvent.onEventCanceled -= MoveOldPosition;

        getOnCamEvent.onGetBool -= GetOnCam;

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
