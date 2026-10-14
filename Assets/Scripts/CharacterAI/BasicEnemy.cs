using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] GameObject jumpscare;
    [SerializeField] private int aiLevel; //0-20
    [SerializeField] private float moveSpeed; //How fast the enemy waits before attempting to move

    [SerializeField] private float timeToKill;
    [SerializeField] private int currentPosition; //currentPosition will go up every successful move attempt

    [SerializeField] private DoorInteractable doorToAppearOn;
    private int cameraIndex;
    private int currentCameraIndex;

    private bool atDoor;
    public GameObject[] characterPositions; //Array of character positions/poses for the enemy to "move" to, will also be used to track if enemy is in Office
    public int[] cameraIndexToShowOn; // This isnt really good. This is an array of what cams the enemy will show on

    private float timer;
    [SerializeField] private GetActiveCamBoolEvent currentCamEvent;
    private void Start()
    {
        TurnOffAllPosition();
        currentCameraIndex = 0;
        currentPosition = 0; //The characters starting position

        cameraIndex = cameraIndexToShowOn[currentCameraIndex];
        characterPositions[currentPosition].SetActive(true); //Set the starting position of the enemy

        //StartCoroutine(GameplayLoop());
    }

    private void Update()
    {
        if (atDoor) return;
        timer += Time.deltaTime;
        if (timer >= moveSpeed)
        {
            int randomNumber = Random.Range(1, 21); //Random number between 1 and 20, Max exclusive
            if (randomNumber <= aiLevel) ChangePosition(); //For example, if aiLevel is 10, and randomNumber is 5, the enemy will move
            timer = 0;
        }
    }

    private void TurnOffAllPosition()
    {
        for (int i = 0; i < characterPositions.Length; i++)
        {
            characterPositions[i].SetActive(false);
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
        if (!doorToAppearOn.isDoorClosed())
        {
            characterPositions[currentPosition].SetActive(false);
            GameManager.Instance.SwitchToGameOver();
            jumpscare.SetActive(true);
        }
        else
        {
            characterPositions[currentPosition].SetActive(false);
            currentCameraIndex = Random.Range(0, cameraIndexToShowOn.Length);
            cameraIndex = cameraIndexToShowOn[currentCameraIndex];
            currentPosition = currentCameraIndex;
            characterPositions[currentPosition].SetActive(true);
            atDoor = false;
        }
    }

    private void ChangePosition()
    {
        if (currentPosition < characterPositions.Length - 1) // Checks if within array
        {
            characterPositions[currentPosition].SetActive(false);
            currentPosition++;
            characterPositions[currentPosition].SetActive(true);
        }

        if (currentCameraIndex < cameraIndexToShowOn.Length - 1) // Checks if within array
        {
            Debug.Log("inside range");
            currentCameraIndex++;
            cameraIndex = cameraIndexToShowOn[currentCameraIndex];
        }

        if (currentPosition == characterPositions.Length - 1) // Checks if within array
        {
            atDoor = true;
            Debug.Log("At door waiting to kill");
            StartCoroutine(WaitToKill());
        }
    }
}