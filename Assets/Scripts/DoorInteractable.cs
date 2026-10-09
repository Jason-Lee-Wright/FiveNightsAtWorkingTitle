using UnityEngine;

public class DoorInteractable : CameraInteractable
{
    [SerializeField] Animator doorAnimator;
    private bool isClosed;

    public bool GetDoorStatus()
    {
        return isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isClosed = true;
            doorAnimator.SetBool("isClosed",isClosed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isClosed = false;
            doorAnimator.SetBool("isClosed", isClosed);
        }
    }
}
