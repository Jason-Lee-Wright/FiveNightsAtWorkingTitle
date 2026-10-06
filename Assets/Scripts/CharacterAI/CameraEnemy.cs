using System.Collections;
using UnityEngine;

public class CameraEnemy : MonoBehaviour
{
    private float patience;
    private float[] patiencePerNight = { 1000, 1000, 1000, 1000, 1000 }; //Will change these later if we add more nights
    
    [SerializeField] private int currentNightIndex;

    private void Start()
    {
        patience = patiencePerNight[currentNightIndex];

        StartCoroutine(GameplayLoop());
    }

    IEnumerator GameplayLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.25f);

            //Commenting out for now until I figure out best way for scripts to talk to each other
            //If(cameraSystem.currentCameraIndex != cameraToWatch) 
            //{
            patience -= .25f;
            //}
            //else patience = patiencePerNight[currentNightIndex]; //Reset patience if the player is looking at the correct camera

            if (patience <= 0) Debug.Log("Game Over"); //Probably won't be instant like this, will update once I get the script fully working
        }
    }
}