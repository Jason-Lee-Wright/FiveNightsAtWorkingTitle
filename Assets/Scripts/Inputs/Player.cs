using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] PlayerInputActions playerInput;
    #region Head Settings
    [SerializeField] float turnSpeed;
    [SerializeField] float maxTurnAngleY;
    [SerializeField] float maxTurnAngleX;
    private float pitchY;
    private float pitchX;
    private Vector2 lookInput;

    #endregion
    private Camera playerHead; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHead = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHeadMovement();
    }

    private void HandleHeadMovement()
    {
        pitchX += lookInput.x * turnSpeed * Time.deltaTime;
        pitchX = Mathf.Clamp(pitchX, -maxTurnAngleX, maxTurnAngleX);

        pitchY -= lookInput.y * turnSpeed * Time.deltaTime;
        pitchY = Mathf.Clamp(pitchY, -maxTurnAngleY, maxTurnAngleY);


        playerHead.transform.localEulerAngles = new Vector3(pitchY, pitchX, 0);
    }

    private void SetLookInput(Vector2 inputValue)
    {
        lookInput = inputValue.normalized;
    }

    private void OnEnable()
    {
        playerInput.lookEvent += SetLookInput;
    }

    private void OnDisable()
    {
        playerInput.lookEvent -= SetLookInput;
    }
}
