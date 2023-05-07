using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 5f;
    public float changeDirectionDelay = 2f;
    public float pauseDuration = 1f;
    public Animator anim;
    private float timeSinceLastDirectionChange = 0f;
    private float timeSinceLastPause = 0f;
    private bool isPaused = false;
    private Vector3 nextDirection;
    public float health = 100f;
    private bool isMoving = false;
    void Update()
    {
        if (!isPaused)
        {

            timeSinceLastDirectionChange += Time.deltaTime;

            if (timeSinceLastDirectionChange >= changeDirectionDelay)
            {
                // Pause before changing direction
                isPaused = true;
                timeSinceLastPause = 0f;
                timeSinceLastDirectionChange = 0f;
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
                //Debug.Log("not moving");
            }
            else
            {
                isMoving = true;
                anim.SetBool("isMoving", isMoving);
               // Debug.Log("Moving");
            }

            // Move the NPC
            transform.Translate(nextDirection * speed * Time.deltaTime, Space.World);

            // Smoothly rotate the NPC towards the next direction
            if (nextDirection != Vector3.zero) // check if nextDirection is not zero
            {
                Quaternion targetRotation = Quaternion.LookRotation(nextDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
            }
        }
        else
        {
            // Pause before changing direction
            timeSinceLastPause += Time.deltaTime;

            if (timeSinceLastPause >= pauseDuration)
            {
                // Change direction after pause
                isPaused = false;
                timeSinceLastPause = 0f;

                // Set the next direction randomly
                float angle = Random.Range(0f, 360f);
                nextDirection = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
                isMoving = true;
                anim.SetBool("isMoving", isMoving);
               // Debug.Log("Moving");
            }
        }
    }


    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Weapon"))
    //    {
    //        TakeDamage(10);
    //    }
    //}

    //void TakeDamage(float amount)
    //{
    //    health -= amount;
    //    Debug.Log(health);

    //    if (health <= 0)
    //    {
    //        Die();
    //    }
    //}

    //void Die()
    //{
    //    // Play death animation or particle effect
    //    Destroy(gameObject);
    //}
}