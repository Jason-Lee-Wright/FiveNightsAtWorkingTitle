using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent onEvent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //animator ??= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RaiseEvent()
    {
        onEvent?.Invoke();
    }
}
