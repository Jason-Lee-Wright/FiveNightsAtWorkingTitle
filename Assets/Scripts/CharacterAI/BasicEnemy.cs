using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] Animator animator;
    [Header("Enemy difficulty")]
    [Range(1, 20)]
    [SerializeField] private int aiLevel; //0-20
    [SerializeField] private float moveSpeed; //How fast the enemy waits before attempting to move
    private float timer;

    
    [Header("Being watched timers")]
    private float waitTime;
    [Range (5, 9)]
    [SerializeField] private float maxWaitTime;
    [Range(3, 5)]
    [SerializeField] private float minWaitTime;
    
    [Header("At door timers")]
    [SerializeField] private float timeToKill; // how long the player has to close the door
    [Range(1,2)]
    [SerializeField] private float timeToRetreat; // how long the enemy waits at the door before retreating
    private int currentPosition; //currentPosition will go up every successful move attempt

    private int enemyCamIndex;
    private int currentCamIndex;
    private int nextCameraIndex;

    private bool atDoor;
    private bool canMove;
    private bool lockedOut;

    [Header("Enemy positions/camera arrays")]
    public GameObject[] characterPositions; //Array of character positions/poses for the enemy to "move" to, will also be used to track if enemy is in Office
    public int[] cameraIndexToShowOn; // This isnt really good. This is an array of what cams the enemy will show on

    [Header("Events")]
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
        if (!canMove || atDoor) return;
        CheckNextCamera();
        timer += Time.deltaTime;
        if (timer >= moveSpeed)
        {
            int randomNumber = Random.Range(1, 21); //Random number between 1 and 20, Max exclusive
            if (randomNumber <= aiLevel) ChangePosition(); //For example, if aiLevel is 10, and randomNumber is 5, the enemy will move
            timer = 0;
        }
    }


    IEnumerator WaitToKill()
    {
        yield return new WaitForSeconds(timeToKill);
        onKill?.Invoke();
        characterPositions[currentPosition].SetActive(false);
        animator.SetTrigger("jumpscare");
    }

    IEnumerator HandleWaiting()
    {
        if (atDoor)
        {
            yield return new WaitForSeconds(timeToRetreat);
        }
        else
        {
            waitTime = Random.Range(minWaitTime, maxWaitTime + 1);
            yield return new WaitForSeconds(waitTime);
        }

        if (atDoor)
        {
            waitOrder = null;
            Retreat();
        }
        else
        {
            activeCamBoolEvent.StaticMove();
            ChangePosition();
            if (activeCamBoolEvent.IsActiveCam(enemyCamIndex)) // Just moved to active camera
            {
                waitOrder = null;
                canMove = false;
                waitOrder = StartCoroutine(HandleWaiting());
            }
            else // Just moved to none active camera
            {
                canMove = true;
                waitOrder = null;
            }
        }
    }

    private void Retreat()
    {
        List<int> safeCams = new List<int>();

        for (int i = 0; i < cameraIndexToShowOn.Length; i++)
        {
            int camIndex = cameraIndexToShowOn[i];

            if (!activeCamBoolEvent.IsActiveCam(camIndex))
            {
                safeCams.Add(i);
            }
            else
            {
                Debug.Log($"{camIndex} is not safe");
            }
        }

        if (safeCams.Count == 0) return;

        int randomIndex = Random.Range(0, safeCams.Count);

        currentCamIndex = safeCams[randomIndex];
        enemyCamIndex = cameraIndexToShowOn[currentCamIndex];

        characterPositions[currentPosition].SetActive(false);
        currentPosition = currentCamIndex;
        characterPositions[currentPosition].SetActive(true);
        canMove = true;
        atDoor = false;
    }

    private void CheckNextCamera()
    {
        if (currentCamIndex < cameraIndexToShowOn.Length - 1)
        {
            nextCameraIndex = cameraIndexToShowOn[currentCamIndex + 1];


            if (activeCamBoolEvent.IsActiveCam(nextCameraIndex))
            {
                timer = 0;
                canMove = false;
                if (waitOrder == null) waitOrder = StartCoroutine(HandleWaiting());
            }
        }
        
    }

    private void ChangePosition()
    {
        if (currentCamIndex < cameraIndexToShowOn.Length - 1)  // Checks if within array before adding current camera index
        {
            currentCamIndex++;
            enemyCamIndex = cameraIndexToShowOn[currentCamIndex];
        }

        if (currentPosition < characterPositions.Length - 1) // Checks if within array before adding current position
        {
            characterPositions[currentPosition].SetActive(false);
            currentPosition++;
            characterPositions[currentPosition].SetActive(true);
        }

        if (currentPosition == characterPositions.Length - 1) // Checks if its last index array and would be at the door
        {
            canMove = false;
            atDoor = true;
            enemyCamIndex = -1;
            if (lockedOut)
            {
                if (waitOrder != null)
                {
                    StopCoroutine(waitOrder);
                    waitOrder = null;
                }

                waitOrder = StartCoroutine(HandleWaiting());
            }
            else
            {
                if (killOrder != null)
                {
                    StopCoroutine(killOrder);
                    killOrder = null;
                }
                killOrder = StartCoroutine(WaitToKill());
            }
            
        }
    }

    private void CheckActiveCam(int ActiveCam)
    {
        if (atDoor) return;

        if (ActiveCam == enemyCamIndex)
        {
            canMove = false;
            timer = 0;
            if (waitOrder != null)
            {
                StopCoroutine(waitOrder);
                waitOrder = null;
            }

            waitOrder = StartCoroutine(HandleWaiting());
        }
        else
        {
            canMove = true;
        }
    }

    public void DoorStatus(bool isClosed)
    {
        if (isClosed)
        {
            lockedOut = true;

            if (killOrder != null)
            {
                StopCoroutine(killOrder);
                killOrder = null;
            }

            if (waitOrder != null)
            {
                StopCoroutine(waitOrder);
                waitOrder = null;
            }
            if (atDoor) waitOrder = StartCoroutine(HandleWaiting());
        }
        else
        {
            lockedOut = false;

            if (atDoor)
            {
                if (killOrder != null)
                {
                    StopCoroutine(killOrder);
                    killOrder = null;
                }
                killOrder = StartCoroutine(WaitToKill());
            }
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