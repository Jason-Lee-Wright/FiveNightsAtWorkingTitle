using System.Collections;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform[] cameras; //Manually setting up camera positions in the inspector
    private int currentCameraIndex;
    private bool onCams;

    [SerializeField] private IntEvent SetCamIndex;
    [SerializeField] private int maxLeftRotation;
    [SerializeField] private int maxRightRotation;
    [SerializeField] private float rotationSpeed;

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
    }

    private void OnEnable()
    {
        SetCamIndex.onEvent += OnCameraChange;
    }

    private void OnDisable()
    {
        SetCamIndex.onEvent -= OnCameraChange;
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