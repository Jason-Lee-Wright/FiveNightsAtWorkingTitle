using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform[] cameras; //Manually setting up camera positions in the inspector
    private int currentCameraIndex;
    private bool onCams;

    [SerializeField] private Transform activeCam;
    [SerializeField] private IntEvent SetCamIndex;

    private void Start()
    {
        currentCameraIndex = 0;
        onCams = false;
        activeCam.position = cameras[currentCameraIndex].position;
    }

    //Method to be put on buttons
    public void OnCameraChange(int nextCameraIndex)
    {
        currentCameraIndex = nextCameraIndex;
        activeCam.position = cameras[currentCameraIndex].position;
    }

    private void OnEnable()
    {
        SetCamIndex.onEvent += OnCameraChange;
    }

    private void OnDisable()
    {
        SetCamIndex.onEvent -= OnCameraChange;
    }
}