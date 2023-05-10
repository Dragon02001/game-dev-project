using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public float followDistance = 15f; // the distance at which the NPC starts following the player
    public float attackDistance = 5f; // the distance at which the NPC starts attacking the player
    public float attackInterval = 1f; // minimum time between attacks
    public float health = 1f;
    public float speed = 5f;
    public AudioClip runSound;
    public AudioClip attackSound;
    public Animator anim;

    private NavMeshAgent agent;
    private GameObject player;
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private float timeSinceLastDirectionChange = 0f;
    private float timeSinceLastPause = 0f;
    private bool isPaused = false;
    private Vector3 nextDirection;
    public float changeDirectionDelay = 2f;
    public float pauseDuration = 1f;
    private bool isMoving = false;
    private bool isAttacking = false;
    private float timeSinceLastAttack = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on game object: " + gameObject.name);
        }
        else if (!agent.isOnNavMesh)
        {
            Debug.LogError("Game object " + gameObject.name + " is not on a NavMesh.");
        }
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource1 = GetComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }


    void Update()
    {

        //Debug.Log(isAttacking);
       // Debug.Log("move "isMoving);
       // Debug.Log("pause "isPaused);

        if (isAttacking)
        {
            // Don't move or attack again until the attack animation is finished
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                isAttacking = false;
                timeSinceLastAttack = 0f;
            }
            return;
        }

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
                // Set the destination to the player's position
                agent.SetDestination(player.transform.position);

                if (!audioSource1.isPlaying)
                {
                    audioSource1.clip = runSound;
                    audioSource1.Play();
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
    

    // Count the time since the last attack
    timeSinceLastAttack += Time.deltaTime;
    }

    IEnumerator AttackWithDelay(float delay)
    {
        // Stop moving and playing the run sound
        agent.isStopped = true;
        if (audioSource1.isPlaying)
        {
            audioSource1.Stop();
        }

        // Play attack sound
        audioSource2.clip = attackSound;
        audioSource2.Play();

        // Trigger attack animation
        anim.SetTrigger("isAttacking");
        isAttacking = true;

        // Inflict damage to player
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            GameObject character = GameObject.FindWithTag("Player"); //Jack prefab is tagged as player
            if (character != null)
            {
                CharacterMovement characterMovement = character.GetComponent<CharacterMovement>();
                characterMovement.TakeDamage(0.1f);
            }
        }

        // Wait for attack animation to finish
        float attackAnimationDuration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(delay);
        StartCoroutine(WaitForAttackAnimation(attackAnimationDuration));
    }

    void attack()
    {
        if (timeSinceLastAttack >= attackInterval)
        {
            StartCoroutine(AttackWithDelay(0.5f)); // Wait for 0.5 seconds before attacking
        }
    }

    IEnumerator WaitForAttackAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        agent.isStopped = false; // Resume moving
        isAttacking = false;
        timeSinceLastAttack = 0f;
        anim.SetBool("isAttacking", false); // Reset the "isAttacking" parameter in the animator
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log(health);

        if (health <= 0)
        {
            Debug.Log(" I am Dead");
            Destroy(gameObject);
        }
    }



}


