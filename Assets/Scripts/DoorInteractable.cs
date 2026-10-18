using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : CameraInteractable
{
    [SerializeField] Animator doorAnimator;
    [SerializeField] UnityEvent<bool> onDoorChange;
    private bool isClosed;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip openningSound;
    [SerializeField] private AudioClip closingSound;
    private void Start()
    {
        source ??= GetComponent<AudioSource>();
        doorAnimator.Play("DoorOpening", 0, 1f);// so they don't play the open door animation at the start
        doorAnimator.GetNextAnimatorStateInfo(0);
    }
    public bool isDoorClosed()
    {
        return isClosed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            ChangeDoorState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            ChangeDoorState(false);
        }
    }

    private void ChangeDoorState(bool newState)
    {
        isClosed = newState;

        doorAnimator.SetBool("isClosed", isClosed);

        if (isClosed)
        {
            source.PlayOneShot(closingSound);
        }
        else
        {
            source.PlayOneShot(openningSound);
        }

        onDoorChange?.Invoke(isClosed);
    }
}
