using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Get Transform Event", menuName = "Events/Get Transform Event")]
public class GetTransformEvent : ScriptableObject
{
    public Func<Transform> onGetTransform;

    public Transform GetTransform() => onGetTransform?.Invoke() ?? null;
}