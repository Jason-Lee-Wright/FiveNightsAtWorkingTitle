using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Return Bool Event", menuName = "Events/Return Bool Event")]
public class GetBoolEvent : ScriptableObject
{
    /// <summary>
    /// Sub to the bool to be return, Example : getBoolEvent.onGetBool += returned bool method
    /// </summary>
    public Func<bool> onGetBool;

    public bool GetBool() //=> onGetBool?.Invoke() ?? false;
    {
        if (onGetBool != null)
        {
            return onGetBool.Invoke();
        }

        return false;
    }
}