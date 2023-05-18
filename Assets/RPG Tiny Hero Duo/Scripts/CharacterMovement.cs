using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;
    public Transform cameraTransform; // Reference to the camera's transform
    
    
    public AudioClip deathSound;
    public AudioClip jumpSound;
    public AudioClip victorySound;
    public AudioClip runningSound;
    public AudioClip dizzySound;
    private AudioSource audioSource;

    public Animator animator;
    private bool isWalking = false;
    public bool isAttacking = false;
    public bool isDefending = false;
    private bool isJumping = false;
    private bool isRunning = false;


    public float MaxHealth = 1.0f;
    public float playerHealth = 1.0f;

    private bool isDead = false;

    public float playerStamina = 1.0f;

    public float maxStamina = 1.0f;

    private Rigidbody rb;
    private bool isGrounded = true; // Initialize to true if the character starts on the ground
    public float jumpForce = 200f; // Adjust the value as needed
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public LineRenderer raycastDebugLine;
    public Vector3 raycastOffset = new Vector3(0f, 1f, 0f);
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
            isGrounded = Physics.Raycast(transform.position + raycastOffset, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);


            // Visualize the raycast using the LineRenderer
            //raycastDebugLine.SetPosition(0, transform.position + raycastOffset);
            //raycastDebugLine.SetPosition(1, transform.position + raycastOffset + Vector3.down * groundCheckDistance);


            UpdateStamina();

            // Move the character forward or backward when the user presses the W or S key
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical != 0 || moveHorizontal != 0)
            {
                Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
                movement = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y, 0.0f) * movement;
                transform.Translate(movement * speed * Time.deltaTime, Space.World);
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // Set the walking parameter in the animator controller based on whether the character is walking or not
            animator.SetBool("isWalking", isWalking);


            // Set the defending parameter in the animator controller based on whether the character is defending or not
            if (Input.GetMouseButtonDown(1)) // 1 represents the right mouse button
            {
                isDefending = true;
            }

            if (Input.GetMouseButtonUp(1)) // 1 represents the right mouse button
            {
                isDefending = false;
            }
            animator.SetBool("isDefending", isDefending);

            // Set the jumping parameter in the animator controller based on whether the character is jumping or not
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && playerStamina > 0.0f)
            {
                audioSource.PlayOneShot(jumpSound);
                isJumping = true;
                isGrounded = false;
                playerStamina -= 0.1f;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
                audioSource.Stop();
            }
            animator.SetBool("isJumping", isJumping);

            // Set the running parameter in the animator controller based on whether the character is running or not
            if (Input.GetKeyDown(KeyCode.LeftShift) && playerStamina > 0.0f)
            {
                speed = 8.0f;
                isRunning = true;
                audioSource.PlayOneShot(runningSound);
                playerStamina -= 0.05f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 5.0f;
                isRunning = false;
                audioSource.Stop();
            }
            animator.SetBool("isRunning", isRunning);

            // Rotate the character based on mouse movement
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, mouseX);

            // Rotate the camera based on the character's rotation
            if (cameraTransform != null)
            {
                cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles.x,
                    cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
            }

            //testing if won
            if (Input.GetKeyDown(KeyCode.O))
            {
                animator.SetBool("isVictorious", true); //Play victory animation
                audioSource.PlayOneShot(victorySound);
                playerHealth = 1f;
            }
            else if (Input.GetKeyUp(KeyCode.O))
            {
                animator.SetBool("isVictorious", false);
            }

            //testing health
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isDefending)
                {
                    animator.SetBool("isHitDefending", true);
                    playerHealth -= 0.05f; //Reduce the damage when defending

                    //Check if the player's health is below zero
                    if (playerHealth <= 0)
                    {
                        isDead = true;
                        animator.SetBool("isDead", true); //Play dead animation
                        audioSource.PlayOneShot(deathSound);
                    }
                }
                else
                {
                    animator.SetBool("isHit", true);
                    playerHealth -= 0.1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                animator.SetBool("isHitDefending", false);
                animator.SetBool("isHit", false);
            }

            // Update the player's health
            if (playerHealth <= 0)
            {
                isDead = true;
                animator.SetBool("isDead", true); //Play dead animation
                audioSource.PlayOneShot(deathSound);
            }

            void UpdateStamina()
            {
                if (isRunning || isAttacking  || isJumping)
                {
                    playerStamina -= 0.1f * Time.deltaTime;
                    if (playerStamina < 0.0f)
                    {
                        playerStamina = 0.0f;
                        isRunning = false;
                        isJumping = false;
                        isAttacking = false;
                        isWalking = false;
                        isDefending = false;

                        speed = 5.0f;
                       // audioSource.PlayOneShot(dizzySound);
                    }
                }
                else
                {
                    playerStamina += 0.05f * Time.deltaTime;
                    if (playerStamina > maxStamina)
                    {
                        playerStamina = maxStamina;
                    }
                }

                UpdateStamina2();
            }

            void UpdateStamina2()
            {
                if (playerStamina <= 0.0f)
                {
                    // Play the animation
                    animator.SetBool("isDizzy", true);

                    // Disable movement
                    speed = 0.0f;

                    // Wait for 3 seconds before enabling movement and resetting stamina
                    StartCoroutine(ResetStaminaAfterDelay(3.0f));
                }
                else
                {
                    animator.SetBool("isDizzy", false);
                    speed = isRunning ? 8.0f : 5.0f;
                }

                playerStamina = Mathf.Clamp(playerStamina, 0.0f, maxStamina);
            }

            IEnumerator ResetStaminaAfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);

                playerStamina = maxStamina;
                speed = isRunning ? 8.0f : 5.0f;
            }

        }
    }
    public void TakeDamage(float amount)
    {
        if (isDefending)
        {
            amount = amount / 2;
        }
        playerHealth -= amount;
        // Debug.Log(playerHealth);

        Vector3 offset = new Vector3(0.0f, 2.5f, 1.0f); // Vertical offset from the character
        Vector3 position = transform.position + offset;
        amount = amount * 100;
        if (amount > 50)
        {
            damagepopup.current.CreatePopUp(position, amount.ToString(), Color.red);
        }
        else
        {
            damagepopup.current.CreatePopUp(position, amount.ToString(), Color.yellow);
        }

        if (playerHealth <= 0)
        {
            Debug.Log("Dead");
            // Die();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Heal")
        {
            Debug.Log("Im healed");
            playerHealth = MaxHealth;
        }




    }
}