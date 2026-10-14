using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Active CAM Bool Event", menuName = "Events/Active CAM Bool")]
public class GetActiveCamBoolEvent : ScriptableObject
{
    public Func<int,bool> onActiveCam;

    public bool OnActiveCam(int currentIndex) => onActiveCam?.Invoke(currentIndex) ?? false;
}