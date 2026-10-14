using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraSystem : MonoBehaviour
{
    public Transform[] cameras; //Manually setting up camera positions in the inspector
    private int currentCameraIndex;
    private bool onCams;

    [SerializeField] private IntEvent SetCamIndex;
    [SerializeField] private int maxLeftRotation;
    private float baseLeftRotation;
    [SerializeField] private int maxRightRotation;
    private float baseRightRotation;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private GetActiveCamBoolEvent currentCamEvent;

    private void Start()
    {
        currentCameraIndex = 0;
        onCams = false;
        transform.position = cameras[currentCameraIndex].position;

        StartCoroutine(TurnCamera());
    }

    //Method to be put on buttons
    public void OnCameraChange(int nextCameraIndex)
    {
        currentCameraIndex = nextCameraIndex;
        transform.position = cameras[currentCameraIndex].position;
        transform.rotation = cameras[currentCameraIndex].rotation;
    }

    public bool OnCam(int indexToCheck)
    {
        if (indexToCheck == currentCameraIndex)
        {
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        SetCamIndex.onEvent += OnCameraChange;
        currentCamEvent.onActiveCam += OnCam;
    }

    private void OnDisable()
    {
        SetCamIndex.onEvent -= OnCameraChange;
        currentCamEvent.onActiveCam -= OnCam;
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
}