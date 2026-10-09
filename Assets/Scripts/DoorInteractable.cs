using UnityEngine;

public class DoorInteractable : CameraInteractable
{
    private bool isClosed;

    public bool GetDoorStatus()
    {
        return isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Here player ");
            isClosed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Player Gone");
            isClosed = false;
        }
    }
}
