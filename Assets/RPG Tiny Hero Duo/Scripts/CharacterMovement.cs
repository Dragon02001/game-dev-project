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
    private bool isDefending = false;
    private bool isJumping = false;
    private bool isRunning = false;

    

    public float playerHealth = 1.0f;

    private bool isDead = false;

    public float playerStamina = 1.0f;

    public float maxStamina = 1.0f;



    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
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
            if (Input.GetKeyDown(KeyCode.Q) && playerStamina > 0.0f)
            {
                isDefending = true;
                playerStamina -= 0.1f;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                isDefending = false;
            }
            animator.SetBool("isDefending", isDefending);

            // Set the jumping parameter in the animator controller based on whether the character is jumping or not
            if (Input.GetKeyDown(KeyCode.Space) && playerStamina > 0.0f)
            {
                audioSource.PlayOneShot(jumpSound);
                isJumping = true;
                playerStamina -= 0.1f;
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
                if (isRunning || isAttacking || isDefending || isJumping)
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
        playerHealth -= amount;
       // Debug.Log(playerHealth);

        if (playerHealth <= 0)
        {
            Debug.Log("Dead");
            // Die();
        }
    }
}