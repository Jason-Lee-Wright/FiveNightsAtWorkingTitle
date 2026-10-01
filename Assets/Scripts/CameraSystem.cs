using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Transform[] cameras; //Manually setting up camera positions in the inspector
    private int currentCameraIndex;
    private bool onCams;

    private void Start()
    {
        currentCameraIndex = 0;
        onCams = false;
        transform.position = cameras[currentCameraIndex].position;
    }

    //Method to be put on buttons
    public void OnCameraChange(int nextCameraIndex)
    {
        currentCameraIndex = nextCameraIndex;
        transform.position = cameras[currentCameraIndex].position;
    }
}