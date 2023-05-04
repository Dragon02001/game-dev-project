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

    public AudioClip attackSound;
    public AudioClip deathSound;
    private AudioSource audioSource;

    private Animator animator;
    private bool isWalking = false;
    private bool isAttacking = false;
    private bool isDefending = false;
    private bool isJumping = false;
    private bool isRunning = false;

    private float lastAttackTime = 0.0f;

    public float playerHealth = 1.0f;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
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

            // Set the attacking parameter in the animator controller based on whether the character is attacking or not
            if (Input.GetKeyDown(KeyCode.E) && Time.time - lastAttackTime > 1.0f) // 1 second cooldown
            {
                audioSource.PlayOneShot(attackSound);
                isAttacking = true;
                lastAttackTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                isAttacking = false;
                audioSource.Stop();
            }
            animator.SetBool("isAttacking", isAttacking);

            // Set the defending parameter in the animator controller based on whether the character is defending or not
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isDefending = true;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                isDefending = false;
            }
            animator.SetBool("isDefending", isDefending);

            // Set the jumping parameter in the animator controller based on whether the character is jumping or not
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
            animator.SetBool("isJumping", isJumping);

            // Set the running parameter in the animator controller based on whether the character is running or not
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = 8.0f;
                isRunning = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 5.0f;
                isRunning = false;
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

            //testing health
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isDefending)
                {
                    animator.SetBool("isHitDefending", true);
                    playerHealth -= 0.05f; //Reduce the damage when defending

                    //Check if the player's health is below zero
                    if(playerHealth <= 0) {
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
            }else if (Input.GetKeyUp(KeyCode.I))
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
        }
    }

//    public void TakeDamage(float damage)
//    {
//        if (!isDefending)
//        {
//            animator.SetBool("isHit", true);
//            playerHealth -= damage * 0.5f; //Reduce the damage when defending
//        }
//        else
//        {
//            animator.SetBool("isHit", true);
//            playerHealth -= damage;
//        }
//    }
}