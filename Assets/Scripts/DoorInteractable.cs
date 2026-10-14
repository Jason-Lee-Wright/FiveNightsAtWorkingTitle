using UnityEngine;

public class DoorInteractable : CameraInteractable
{
    [SerializeField] Animator doorAnimator;
    private bool isClosed;

    private void Start()
    {
        doorAnimator.Play("DoorOpening", 0, 1f);// so they don't play the open door animation at the start
    }
    public bool isDoorClosed()
    {
        return isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isClosed = true;
            doorAnimator.SetBool("isClosed", isClosed);
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
