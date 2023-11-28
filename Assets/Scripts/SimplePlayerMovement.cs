using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;          // Movement speed
    public float rotationSpeed = 720f; // Rotation speed in degrees per second

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize(); // Normalize to prevent faster movement diagonally

        // Move the player
        characterController.SimpleMove(movement * speed);

        // Get input for rotation
        float rotationInput = Input.GetAxis("Rotation");

        // Rotate the player
        transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
    }
}
