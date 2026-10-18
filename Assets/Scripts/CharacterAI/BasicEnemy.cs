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
    [Range(1, 3)]
    [SerializeField] private int levelIncrease; // ai level increase before capping
    [Range(1, 20)]
    [SerializeField] private int maxLevel; // level before ai level gets capped 
    [SerializeField] private float moveSpeed; //How fast the enemy waits before attempting to move
    private float moveTimer;
    private float minuteTimer;
    
    [Header("Being watched timers")]
    private float waitTime;
    [Range (5, 9)]
    [SerializeField] private float maxWaitTime;
    [Range(3, 5)]
    [SerializeField] private float minWaitTime;
    
    [Header("At door timers")]
    [Range (1, 5)]
    [SerializeField] private float timeToKill; // how long the player has to close the door
    [Range(1,3)]
    [SerializeField] private float timeToRetreat; // how long the enemy waits at the door before retreating
    private int currentPosition; //currentPosition will go up every successful move attempt

    private int enemyCamIndex;
    private int currentCamIndex;
    private int nextCameraIndex;
    private int activeCam;

    private bool atDoor;
    private bool canMove;
    private bool lockedOut;

    [Header("Enemy positions/camera arrays")]
    public GameObject[] characterPositions; //Array of character positions/poses for the enemy to "move" to, will also be used to track if enemy is in Office
    public int[] cameraIndexToShowOn; // This isnt really good. This is an array of what cams the enemy will show on

    [Header("Events")]
    [SerializeField] private IntEvent camIndexEvent;
    [SerializeField] private UnityEvent onMoveActiveCam; // Used to play static video on cams
    [SerializeField] private UnityEvent onKill;
    [SerializeField] private UnityEvent onLockedOut; // To play knocking audio
    [SerializeField] private UnityEvent onOpenDoor; // To stop knocking
    private Coroutine killOrder;
    private Coroutine waitOrder;
    private void Awake()
    {
        canMove = true;
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
        HandleEnemyTime();
    }

    private void HandleEnemyTime()
    {
        if (aiLevel == maxLevel) return;
        minuteTimer += Time.deltaTime;
        if (minuteTimer >= 60f)
        {
            aiLevel += levelIncrease;
            minuteTimer = 0;
        }
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
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveSpeed)
        {
            int randomNumber = Random.Range(1, 21); //Random number between 1 and 20, Max exclusive
            if (randomNumber <= aiLevel) ChangePosition(); //For example, if aiLevel is 10, and randomNumber is 5, the enemy will move
            moveTimer = 0;
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
            waitOrder = null;
            Retreat();
        }
        else
        {
            waitTime = Random.Range(minWaitTime, maxWaitTime + 1);
            yield return new WaitForSeconds(waitTime);
            canMove = true;
            waitOrder = null;
        }
    }

    private void Retreat()
    {
        List<int> safeCams = new List<int>();

        for (int i = 0; i < cameraIndexToShowOn.Length; i++)
        {
            int camIndex = cameraIndexToShowOn[i];


            if (camIndex != activeCam)
            {
                if (camIndex != characterPositions.Length - 1) // Dont want them on the last camera they apprear on before the door
                {
                    safeCams.Add(i);
                }
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

    private void ChangePosition()
    {
        if (currentCamIndex < cameraIndexToShowOn.Length - 1)  // Checks if within array before adding current camera index
        {
            if (enemyCamIndex == activeCam) // Current active cam
            {
                onMoveActiveCam?.Invoke();
            }
            currentCamIndex++;
            enemyCamIndex = cameraIndexToShowOn[currentCamIndex];

            if (enemyCamIndex == activeCam) // Next index is active cam
            {
                onMoveActiveCam?.Invoke();
            }
        }

        if (currentPosition < characterPositions.Length - 1) // Checks if within array before adding current position
        {
            characterPositions[currentPosition].SetActive(false);
            currentPosition++;
            characterPositions[currentPosition].SetActive(true);
        }

        if (currentPosition == characterPositions.Length - 1) // Checks if its last index array and would be at the door
        {
            if (enemyCamIndex == activeCam)
            {
                onMoveActiveCam?.Invoke();
            }
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

    private void CheckActiveCam(int newCam)
    {
        if (atDoor || activeCam == newCam) return;

        activeCam = newCam;
        if (newCam == enemyCamIndex)
        {
            canMove = false;
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

            if (atDoor) 
            {
                waitOrder = StartCoroutine(HandleWaiting());
                onLockedOut?.Invoke();
            } 
        }
        else
        {
            lockedOut = false;

            if (atDoor)
            {
                onLockedOut?.Invoke();
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