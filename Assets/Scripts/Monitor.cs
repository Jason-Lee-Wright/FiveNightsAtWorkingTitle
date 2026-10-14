using UnityEngine;

public class Monitor : MonoBehaviour // I made this cause this way is just so easier than using the prefab
{
    [SerializeField] GetTransformEvent screenTransformEvent;

    private Transform getTransform() => transform;

    private void OnEnable()
    {
        screenTransformEvent.onGetTransform += getTransform;
    }

    private void OnDisable()
    {
        screenTransformEvent.onGetTransform -= getTransform;
    }
}
