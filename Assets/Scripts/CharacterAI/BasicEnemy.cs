using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Range(1, 20)]
    [SerializeField] private int aiLevel; //0-20
    [SerializeField] private float moveSpeed; //How fast the enemy waits before attempting to move
    private float timer;

    [SerializeField] private float timeToKill;
    [SerializeField] private int currentPosition; //currentPosition will go up every successful move attempt

    [SerializeField] private int enemyCamIndex;
    [SerializeField] private int currentCamIndex;

    private bool atDoor;
    [SerializeField] private bool beingWatched;
    private bool lockedOut;
    public GameObject[] characterPositions; //Array of character positions/poses for the enemy to "move" to, will also be used to track if enemy is in Office
    public int[] cameraIndexToShowOn; // This isnt really good. This is an array of what cams the enemy will show on

    
    [SerializeField] private IntEvent camIndexEvent;
    [SerializeField] private GetActiveCamBoolEvent activeCamBoolEvent;
    [SerializeField] private UnityEvent onKill;
    private Coroutine killOrder;
    private Coroutine waitOrder;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        TurnOffAllPosition();
        currentCamIndex = 0;

        enemyCamIndex = cameraIndexToShowOn[currentCamIndex];
        characterPositions[currentPosition].SetActive(true); //Set the starting position of the enemy

        //StartCoroutine(GameplayLoop());
    }

    private void Update()
    {
        HandleMovement();
    }

    private void TurnOffAllPosition()
    {
        for (int i = 0; i < characterPositions.Length; i++)
        {
            characterPositions[i].SetActive(false);
        }
    }

    private void HandleMovement()
    {
        if (atDoor || beingWatched) return;

        timer += Time.deltaTime;
        if (timer >= moveSpeed)
        {
            int randomNumber = Random.Range(1, 21); //Random number between 1 and 20, Max exclusive
            if (randomNumber <= aiLevel) ChangePosition(); //For example, if aiLevel is 10, and randomNumber is 5, the enemy will move
            timer = 0;
        }
    }
    
    IEnumerator GameplayLoop() // This freezes the game if i cancel it
    {
        while (true)
        {
            yield return new WaitForSeconds(moveSpeed);
            Debug.Log("timer done");
            //if (!currentCamEvent.OnActiveCam(cameraIndex))
            int randomNumber = Random.Range(1, 21); //Random number between 1 and 20, Max exclusive
            if (randomNumber <= aiLevel) ChangePosition(); //For example, if aiLevel is 10, and randomNumber is 5, the enemy will move
        }
    }
    IEnumerator WaitToKill()
    {
        yield return new WaitForSeconds(timeToKill);
        onKill?.Invoke();
        characterPositions[currentPosition].SetActive(false);
        animator.SetTrigger("jumpscare");
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(moveSpeed);
        RandomPosition();
    }

    private void RandomPosition()
    {
        characterPositions[currentPosition].SetActive(false);

        currentCamIndex = Random.Range(0, cameraIndexToShowOn.Length);
        enemyCamIndex = cameraIndexToShowOn[currentCamIndex];
        enemyCamIndex = currentCamIndex;
        characterPositions[currentPosition].SetActive(true);
        atDoor = false;
    }

    private void ChangePosition()
    {
        if (currentPosition < characterPositions.Length - 1) // Checks if within array before adding current position
        {
            characterPositions[currentPosition].SetActive(false);
            currentPosition++;
            characterPositions[currentPosition].SetActive(true);
        }

        if (currentCamIndex < cameraIndexToShowOn.Length - 1)  // Checks if within array before adding current camera index
        {
            currentCamIndex++;
            enemyCamIndex = cameraIndexToShowOn[currentCamIndex];
        }

        if (currentPosition == characterPositions.Length - 1) // Checks if its last index array
        {
            atDoor = true;
            enemyCamIndex = -1;
            killOrder = StartCoroutine(WaitToKill());
        }
    }

    private void CheckActiveCam(int ActiveCam)
    {
        if (ActiveCam == enemyCamIndex)
        {
            Debug.Log($"{gameObject.name} is being watched");
            beingWatched = true;
        }
        else
        {
            Debug.Log($"{gameObject.name} is not being watched");
            beingWatched = false;
        }
    }

    public void DoorStatus(bool isClosed)
    {
        if (isClosed)
        {
            lockedOut = true;
            if (killOrder != null) StopCoroutine(killOrder);
            waitOrder = StartCoroutine(WaitToMove());
        }
    }

    private void OnEnable()
    {
        camIndexEvent.onEvent += CheckActiveCam;
    }
    private void OnDisable()
    {
        camIndexEvent.onEvent -= CheckActiveCam;
    }
}