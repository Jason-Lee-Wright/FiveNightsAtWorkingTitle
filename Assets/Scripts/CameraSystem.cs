using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Video;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private Transform[] cameras; //Manually setting up camera positions in the inspector
    [SerializeField] private GameObject activeCam;
    [SerializeField] private int cameraStartIndex;
    private int currentCameraIndex;

    [SerializeField] private IntEvent SetCamIndex;
    [SerializeField] private GetActiveCamBoolEvent currentCamEvent;

    [SerializeField] private UnityEvent onCamChange;
    [Header("Cam turning setting")]
    [SerializeField] private int maxLeftRotation;
    private float baseLeftRotation;
    [SerializeField] private int maxRightRotation;
    private float baseRightRotation;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private UnityEvent onStaticStart;
    [SerializeField] private UnityEvent onStaticEnd;
    [SerializeField] private float staticTime;
    private Coroutine staticRoutine;

    [SerializeField] GetTransformEvent screenTransformEvent;

    private Transform GetTransform() => transform;

    private void Awake()
    {
        SetCamIndex.RaiseEvent(cameraStartIndex);

        StartCoroutine(TurnCamera());
    }

    //Method to be put on buttons
    public void OnCameraChange(int nextCameraIndex)
    {
        if (currentCameraIndex == nextCameraIndex) return; 
        currentCameraIndex = nextCameraIndex;
        if (activeCam  != null )
        {
            activeCam.transform.position = cameras[currentCameraIndex].position;
            activeCam.transform.rotation = cameras[currentCameraIndex].rotation;
        }
        onCamChange?.Invoke();
        staticRoutine = StartCoroutine(PlayStaticVideo());
    }

    public bool CheckOnCams(int indexToCheck)
    {
        if (indexToCheck == currentCameraIndex)
        {
            return true;
        }

        return false;
    }

    private IEnumerator PlayStaticVideo()
    {
        onStaticStart?.Invoke();
        yield return new WaitForSeconds(staticTime);
        onStaticEnd?.Invoke();
    }

    private IEnumerator TurnCamera()
    {
        bool rotatingRight = true;

        while (true)
        {
            float yRotation = transform.eulerAngles.y;

            if (yRotation > 180f) yRotation -= 360f;

            if (rotatingRight)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

                if (yRotation >= maxRightRotation) rotatingRight = false;
            }
            else
            {
                transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);

                if (yRotation <= maxLeftRotation) rotatingRight = true;
            }

            yield return null;
        }
    }

    private void OnEnable()
    {
        SetCamIndex.onEvent += OnCameraChange;
        currentCamEvent.onActiveCam += CheckOnCams;
        if (screenTransformEvent != null) screenTransformEvent.onGetTransform += GetTransform;
    }

    private void OnDisable()
    {
        SetCamIndex.onEvent -= OnCameraChange;
        currentCamEvent.onActiveCam -= CheckOnCams;
        if (screenTransformEvent != null) screenTransformEvent.onGetTransform -= GetTransform;
    }
}