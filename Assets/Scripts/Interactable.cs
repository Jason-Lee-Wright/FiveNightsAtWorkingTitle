using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;
    public virtual void OnInteract()
    {
        onInteract?.Invoke();
    }
}
