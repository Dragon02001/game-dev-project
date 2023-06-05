using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 5f;  // The movement speed of the object

    private void Update()
    {
        // Get the input axes values for horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input axes
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalize the movement direction to prevent diagonal movement from being faster
        movementDirection.Normalize();

        // Move the object based on the normalized movement direction and speed
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }
}
