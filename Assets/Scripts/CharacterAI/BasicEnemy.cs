using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [SerializeField] private int aiLevel; //0-20
    [SerializeField] private float moveSpeed; //How fast the enemy waits before attempting to move
    [SerializeField] private int currentPosition; //currentPosition will go up every successful move attempt
    
    public GameObject[] characterPositions; //Array of character positions/poses for the enemy to "move" to, will also be used to track if enemy is in Office

    private void Start()
    {
        currentPosition = 0;

        characterPositions[currentPosition].SetActive(true); //Set the starting position of the enemy

        StartCoroutine(GameplayLoop());
    }

    IEnumerator GameplayLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveSpeed);
            int randomNumber = Random.Range(1, 21);
            if (randomNumber <= aiLevel) ChangePosition();
        }
    }

    private void ChangePosition()
    {
        characterPositions[currentPosition].SetActive(false);
        currentPosition++;
        characterPositions[currentPosition].SetActive(true);

        if(currentPosition == characterPositions.Length) Debug.Log("Game Over");
    }
}