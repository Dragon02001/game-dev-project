using UnityEngine;

namespace Cinemachine.Examples
{
    [AddComponentMenu("")] // Don't display in add component menu
    public class CharacterMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float turnSpeed = 10f;
        public KeyCode sprintJoystick = KeyCode.JoystickButton2;
        public KeyCode sprintKeyboard = KeyCode.Space;

        private Vector2 input;
        private Camera mainCamera;

        // Use this for initialization
        void Start()
        {
            mainCamera = Camera.main;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            // Calculate movement direction based on input and camera orientation
            Vector3 moveDirection = CalculateMoveDirection();

            // Calculate movement speed based on sprinting input
            float speed = moveSpeed;
            if (Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard))
            {
                speed *= 2f; // Double the speed when sprinting
            }

            // Apply movement
            transform.position += moveDirection * speed * Time.deltaTime;

            // Rotate character towards the movement direction
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }

        private Vector3 CalculateMoveDirection()
        {
            Vector3 moveDirection = Vector3.zero;

            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            var right = mainCamera.transform.TransformDirection(Vector3.right);

            moveDirection = input.x * right + input.y * forward;
            moveDirection.Normalize();

            return moveDirection;
        }
    }
}
