using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runspeed = 10f;
    public float changeDirectionDelay = 2f;
    public float pauseDuration = 1f;
    public Animator anim;
    public float health = 100f;

    private float timeSinceLastDirectionChange = 0f;
    private float timeSinceLastPause = 0f;
    private bool isPaused = false;
    private Vector3 nextDirection;
    private bool isMoving = false;
    private GameObject player;
    private float followDistance = 15f; // the distance at which the NPC starts following the player
    private float attackDistance = 5f;// the distance at which the NPC starts attacking the player
    public float attackInterval = 1f; // Minimum time between attacks

    public AudioClip runSound;
    public AudioClip attackSound;
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource1 = GetComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the player is within the follow distance
        if (Vector3.Distance(transform.position, player.transform.position) < followDistance)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
            {
                attack();
            }
            else
            {
                anim.SetBool("agro", true);
                // Set the next direction towards the player
                nextDirection = (player.transform.position - transform.position).normalized;

                
                if (!audioSource1.isPlaying)
                {
                    audioSource1.clip = runSound;
                    audioSource1.Play();
                }
                // Move the NPC towards the player
                transform.Translate(nextDirection * runspeed * Time.deltaTime, Space.World);

                // Smoothly rotate the NPC towards the player
                if (nextDirection != Vector3.zero) // check if nextDirection is not zero
                {
                    Quaternion targetRotation = Quaternion.LookRotation(nextDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
                }
            }
        }
        else if (!isPaused && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // Stop playing the audio when the spider is not following the character
            if (audioSource1.isPlaying)
            {
                audioSource1.Stop();
            }
            anim.SetBool("agro", false);
            timeSinceLastDirectionChange += Time.deltaTime;

            if (timeSinceLastDirectionChange >= changeDirectionDelay)
            {
                // Pause before changing direction
                isPaused = true;
                timeSinceLastPause = 0f;
                timeSinceLastDirectionChange = 0f;
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
            }
            else
            {
                isMoving = true;
                anim.SetBool("isMoving", isMoving);
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
            // Stop playing the audio when the spider is not following the character
            if (audioSource1.isPlaying)
            {
                audioSource1.Stop();
            }
            anim.SetBool("agro", false);
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
            }
        }
    }


    void attack()
    {
        // Stop moving
        isMoving = false;
        anim.SetBool("isMoving", isMoving);
        if (audioSource1.isPlaying)
        {
            audioSource1.Stop();
        }

        // Play attack sound
        audioSource2.clip = attackSound;
        audioSource2.Play();
        // Trigger attack animation
        anim.SetTrigger("isAttacking");

        

        // Wait for attack animation to finish
        float attackAnimationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(WaitForAttackAnimation(attackAnimationDuration));
    }

    IEnumerator WaitForAttackAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        // Resume moving
        isMoving = true;
        anim.SetBool("isMoving", isMoving);
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
