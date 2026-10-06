using UnityEngine;

public class DoorInteractable : CameraInteractable
{
    private bool isClosed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInteract()
    {
        base.OnInteract();
        isClosed = true;
    }

    public override void Cancel()
    {
        base.Cancel();
        isClosed = false;
    }

    public bool GetDoorStatus()
    {
        return isClosed;
    }
}
