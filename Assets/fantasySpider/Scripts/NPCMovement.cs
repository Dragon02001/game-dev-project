using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{

  
    [SerializeField] private float followDistance = 15f;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;

    private bool isTriggered = false;
    private bool ultTrigger = false;
    private bool sideTrigger = false;
    private bool Alive = true;
    private bool Freeze = false;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isAggressive = false;
    private bool isHit = false;
    private bool isPaused = false;
    private bool onFire = false;
    private float timeSinceLastAttack = 0f;
    private float timeSinceLastDirectionChange = 0f;
    private float timeSinceLastPause = 0f;
    private float directionChangeDelay = 2f;
    private float pauseDuration = 1f;
    private float currentHealth;
    public float PlayerHealth;
    public int Fire;
    public int Ice;

    private Vector3 nextDirection;
  

    private void Start()
    {
        // Get the Renderer component attached to the same GameObject

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

        currentHealth = maxHealth;

      

}

    private void Update()
    {
        if (Alive)
        {
          if (!Freeze)
           {
            if (isAttacking)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    isAttacking = false;
                    timeSinceLastAttack = 0f;
                    agent.isStopped = false;
                }
                return;
            }
            else
            {
                ResumeMovement();
            }
                if (Vector3.Distance(transform.position, player.transform.position) <= followDistance && !isAttacking)
                {
                    isMoving = true;
                    animator.SetBool("isMoving", isMoving);
                    audioSource1.clip = runSound;
                   // audioSource1.loop = true;
                    if (!audioSource1.isPlaying && !isAttacking)
                    {
                        audioSource1.Play();
                    }
                    isAggressive = true;
                }
                else
                {
                    isMoving = false;
                    animator.SetBool("isMoving", isMoving);
                    agent.ResetPath();
                    isAggressive = false;
                    if (audioSource1.isPlaying)
                    {
                        audioSource1.Stop();
                    }
                }

                if (isAggressive)
            {
                CharacterMovement characterMovement = player.GetComponent<CharacterMovement>();
                PlayerHealth = characterMovement.playerHealth;
                // Check if the player is within attack range
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                        if (audioSource1.isPlaying)
                        {
                            audioSource1.Stop();
                        }

                        // Attack the player if enough time has passed since the last attack
                        if (timeSinceLastAttack >= attackInterval)
                    {

                        // Start attacking with delay
                        if (PlayerHealth > 0)
                        {
                            StartCoroutine(AttackWithDelay(0.5f));
                        }
                        else
                        {
                            animator.SetTrigger("Taunt");
                        }
                    }



                }
                else
                {
                    if (PlayerHealth > 0)
                    {
                        // Set the destination to the player's position
                        agent.SetDestination(player.transform.position);
                    }
                }
            }
            else
            {
                // Stop playing the audio when the spider is not following the character
                if (audioSource1.isPlaying)
                {
                    audioSource1.Stop();
                }

                // Roam around randomly
                if (!isPaused && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && currentHealth > 0)
                {
                    timeSinceLastDirectionChange += Time.deltaTime;
                    if (timeSinceLastDirectionChange >= directionChangeDelay)
                    {
                        // Pause before changing direction
                        isPaused = true;
                        timeSinceLastPause = 0f;
                        timeSinceLastDirectionChange = 0f;
                        isMoving = false;
                        animator.SetBool("isMoving", isMoving);
                    }
                    else
                    {
                        isMoving = true;
                        animator.SetBool("isMoving", isMoving);
                    }

                    // Move the NPC
                    transform.Translate(nextDirection * speed * Time.deltaTime, Space.World);

                    if (nextDirection != Vector3.zero)
                    {
                        // Rotate the NPC towards the next direction
                        Quaternion targetRotation = Quaternion.LookRotation(nextDirection, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
                    }
                }
                else if (currentHealth > 0)
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
                        animator.SetBool("isMoving", isMoving);
                    }
                }
            }

            // Update the time since last attack
            timeSinceLastAttack += Time.deltaTime;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            Vector3 offset = new Vector3(0.0f, 2.5f, 1.0f); // Vertical offset from the character
            Vector3 position = transform.position + offset;
            damage = damage * 100;
            if (damage > 50)
            {
                damagepopup.current.CreatePopUp(position, damage.ToString(), Color.red);
            }
            else
            {
                damagepopup.current.CreatePopUp(position, damage.ToString(), Color.yellow);
            }
            // Play hit animation and sound
            //animator.SetTrigger("Hit");
            // audioSource2.clip = hitSound;
            // audioSource2.Play();
        }
        // If the NPC's health is depleted, trigger death sequence

        if (currentHealth <= 0)
        {
            if (Alive == true)
            {
                Alive = false;
                isAggressive = false;
                isMoving = false;
                animator.SetBool("isMoving", isMoving);
                animator.SetTrigger("Die");
                agent.isStopped = true;
                audioSource1.Stop();
                // audioSource2.clip = deathSound;
                // audioSource2.Play();
                Destroy(gameObject, 3f);
            }
        }
        Debug.Log(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "FreezeCircle" && !ultTrigger)
        {
            Ice = 2;
            ultTrigger = true;
            Debug.Log("its cold");
            FreezeNpc();
            TakeDamage(0.5f);
            
        }
        if (other.tag == "IceSlash" && !sideTrigger)
        {
            Ice = 1;
            sideTrigger = true;
            Debug.Log("its cold");
            FreezeNpc();
            TakeDamage(0.3f);
            
        }
 
        if (other.tag == "FireSlash" && !sideTrigger)
        {
            Fire = 1;
            sideTrigger = true;
            Debug.Log("its Hot");
            onFireNPC();


        }
        if (other.tag == "HellCircle" && !ultTrigger)
        {
            Fire = 2;
            ultTrigger = true;
            Debug.Log("its Hot");
            onFireNPC();

        }


    }
    //******************************
    void restoreUlt()
    {
        ultTrigger = false;
    }
    void restoreSide()
    {
        sideTrigger = false;
    }
    //Fire Methods
    void NotOnFire()
    {
        onFire = false;
        
    }
    void onFireNPC()
    {
        onFire = true;
        if (Fire == 1)
        {
            burn();
            Invoke("restoreSide", 3f); // Restore trigger
        }
        if (Fire == 2)
        {
            evaporate();
            Invoke("restoreUlt", 3f); // Restore trigger
        }
        Invoke("NotOnFire", 3f); // Remove burn effect
    }
    void evaporate()
    {
        if (onFire == true)
        {
            TakeDamage(0.05f);
            Invoke("evaporate", 0.25f);
        }
    }
    void burn()
    {
        if (onFire == true)
        {
            TakeDamage(0.25f);
            Invoke("burn", 1.1f);
        }
    }
    //Ice Methods
    void UnFreeze()
    {
        Freeze = false;
    }
    void FreezeNpc()//freezes fir 4 seconds
    {
        Freeze = true;
        isMoving = false;
        animator.SetBool("isMoving", isMoving);
        if (audioSource1.isPlaying)
        {
            audioSource1.Stop();
        }
        if (Ice == 1)
        {
            Invoke("restoreSide", 4f); // Restore agent speed after 4 seconds
        }
        if (Ice == 2)
        {
            Invoke("restoreUlt", 4f); // Restore agent speed after 4 seconds
        }
            
        Invoke("UnFreeze", 4f); // unfreeze npc
    }
    //*****************************
    private void ResumeMovement()
    {
        if (agent.isStopped && !isAttacking)
        {
            agent.isStopped = false;
            if (audioSource1.clip == runSound && !audioSource1.isPlaying && !isAttacking)
            {
                audioSource1.Play();
            }
        }
    }

    private void SetInitialDirection()
    {
        float angle = Random.Range(0f, 360f);
        nextDirection = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
    }

    private void StartRoaming()
    {
        SetInitialDirection();
        isPaused = false;
        timeSinceLastPause = 0f;
        timeSinceLastDirectionChange = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the follow and attack distance spheres in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    IEnumerator AttackWithDelay(float delay)
    {
        // Stop moving and playing the run sound
        agent.isStopped = true;
        //if (audioSource1.isPlaying)
        //{
            audioSource1.Stop();
        //}


        // Trigger attack animation
        int randomNumber = UnityEngine.Random.Range(1, 5);
        Debug.Log(randomNumber);
        if (randomNumber == 1) 
        {
            animator.SetTrigger("Attack1");
        } 
        else if (randomNumber == 2)
        {
            animator.SetTrigger("Attack2");
        }
        else if (randomNumber == 3)
        {
            animator.SetTrigger("Attack3");
        }
        else if (randomNumber == 4)
        {
            animator.SetTrigger("Attack4");
        }

        isAttacking = true;

        // Play attack sound
        audioSource2.clip = attackSound;
        audioSource2.Play();



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



}