using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] phoneCalls;
    [SerializeField] private int currentNightIndex; //0 would be first night

    private AudioSource audioSource;
    private bool isPaused = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = phoneCalls[currentNightIndex];
        audioSource.Play();
    }

    public void OnPause(InputAction.CallbackContext context) //Invoke Unity Events to pause Audio
    {
        if (context.started)
        {
            isPaused = !isPaused;

            //This should just start right where it left off, rather than restarting
            if (isPaused) audioSource.Pause();
            else audioSource.UnPause();
        }
    }
}