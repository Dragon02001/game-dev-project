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
    private float changeDirectionDelay = 2f;
    private float pauseDuration = 1f;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isAgro = false;
    private bool hit = false;
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
        //Debug.Log("isAttacking: " + isAttacking);
        //Debug.Log("isMove: " + isMoving);
        //Debug.Log("isPaused: " + isPaused);
        //Debug.Log("isAgro: " + isAgro);

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
        if (Vector3.Distance(transform.position, player.transform.position) < followDistance && health > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
            {
                attack();
            }
            else
            {
                isAgro = true;
                anim.SetBool("agro", isAgro);
                // Set the destination to the player's position
                agent.SetDestination(player.transform.position);

                if (!audioSource1.isPlaying)
                {
                    audioSource1.clip = runSound;
                    audioSource1.Play();
                }
            }
        }
        else if (!isPaused && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && health > 0)
        {
            // Stop playing the audio when the spider is not following the character
            if (audioSource1.isPlaying)
            {
                audioSource1.Stop();
            }
            isAgro = false;
            anim.SetBool("agro", isAgro);
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
        else if ( health > 0)
        {
            // Stop playing the audio when the spider is not following the character
            if (audioSource1.isPlaying)
            {
                audioSource1.Stop();
            }
            isAgro = false;
            anim.SetBool("agro", isAgro);
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

        yield return new WaitForSeconds(delay);

        // Resume following the player
        agent.isStopped = false;
    }

    void attack()
    {
        if (timeSinceLastAttack > attackInterval)
        {
            // Attack with a delay
            StartCoroutine(AttackWithDelay(0.5f));
            timeSinceLastAttack = 0f;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        GameObject character = GameObject.FindGameObjectWithTag("Enemy");
        Vector3 offset = new Vector3(0.0f, 2.5f, 1.0f); // Vertical offset from the character
        Vector3 position = character.transform.position + offset;
        damagepopup.current.CreatePopUp(position, damage.ToString(), Color.yellow);
        if (health <= 0)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("agro", false);
            anim.SetBool("Hit", false);
            anim.SetBool("isDead", true);
            // Destroy the NPC if health is zero or less
            Destroy(gameObject, 3f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere around the NPC to indicate the attack distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
        // Draw a wire sphere around the NPC to indicate the follow distance
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
}

    