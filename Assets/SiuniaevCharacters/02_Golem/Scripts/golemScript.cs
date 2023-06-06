using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class golemScript : MonoBehaviour
{


    [SerializeField] private float followDistance = 20f;
    [SerializeField] private float attackDistance = 5f;
    [SerializeField] private float attackInterval = 2.5f;
    [SerializeField] private float DamageDone = 0.15f;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip defendedSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private float rotationSpeed = 5f;




    private bool hasEntered = false;
    private int phase = 1;
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
    private float Health;
    public float PlayerHealth;
    public int Fire;
    public int Ice;
    private bool electrified = false; // New bool variable
    private int countOfNpc;
    private Vector3 nextDirection;
    public bool invulnerable = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;


    public GameObject objectToSpawn1; // The object to spawn
    public GameObject objectToSpawn2; // The object to spawn
    public GameObject objectToSpawn3;
    public GameObject objectToSpawn4;
    public GameObject objectToSpawn5;
    public GameObject objectToSpawn6;
    public Transform spawnPoint1; // The position where the object will spawn
    public Transform spawnPoint2; 
    public Transform spawnPoint3; 
    public Transform spawnPoint4; 
    public Transform spawnPoint5; 
    public Transform spawnPoint6; 
    private GameObject spawnedObject1; // Reference to the first spawned object
    private GameObject spawnedObject2; 
    private GameObject spawnedObject3; 
    private GameObject spawnedObject4; 
    private GameObject spawnedObject5; 
    private GameObject spawnedObject6;
    public GameObject prefabToInstantiateA;
    public GameObject newObject1;
    public GameObject newObject2;
    public GameObject newObject3;
    public GameObject newObject4;
    public GameObject newObject5;
    public GameObject newObject6;
    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
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

        Health = maxHealth;



    }


    private void Update()
    {
        if (invulnerable)
        {
            if (!isAggressive && Vector3.Distance(transform.position, initialPosition) > 0.3f)
            {
                // If not aggressive and not reached the initial position yet, play walking animation
                isMoving = true;
                animator.SetBool("isMoving", isMoving);
            }
            else
            {
                // Otherwise, stop the walking animation and face the original direction
                isMoving = false;
                animator.SetBool("isMoving", isMoving);

                // Face the original direction (towards initial rotation)
                Quaternion targetRotation = Quaternion.LookRotation(initialRotation * Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        if (Alive && !invulnerable)
        {
            if (!Freeze)
            {
                if (isAttacking)
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack4"))
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
                if (electrified)
                {
                    GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Enemy");
                    countOfNpc = npcObjects.Length;
                    Debug.Log("Number of objects in npcObjects array: " + countOfNpc);
                    float closestDistance = Mathf.Infinity;
                    GameObject closestNPC = null;
                    Vector3 sourcePosition = transform.position; // Assuming this script is attached to the source object
                    if (countOfNpc > 1)
                    {
                        foreach (GameObject npcObject in npcObjects)
                        {
                            if (npcObject != gameObject) // Exclude the source object itself from consideration
                            {
                                Vector3 npcPosition = npcObject.transform.position;
                                float distance = Vector3.Distance(sourcePosition, npcPosition);

                                if (distance < closestDistance)
                                {
                                    closestDistance = distance;
                                    closestNPC = npcObject;
                                }
                            }
                        }

                        // Use the 'closestNPC' object for further interactions or operations
                        if (closestNPC != null)
                        {
                            player = closestNPC;
                        }
                    }


                }
                else
                {
                    player = GameObject.FindGameObjectWithTag("Player");
                }
                if (Vector3.Distance(transform.position, player.transform.position) <= followDistance && !isAttacking && Health > 0)
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
                    if (electrified)
                    {
                        NPCMovement npcmovement = player.GetComponent<NPCMovement>();
                        PlayerHealth = npcmovement.Health;
                    }
                    else
                    {
                        CharacterMovement characterMovement = player.GetComponent<CharacterMovement>();
                        PlayerHealth = characterMovement.Health;
                    }
                    // Check if the player is within attack range
                    //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                    if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance && Health > 0)
                    {
                        // Debug.Log("following");
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
                                if (!electrified)
                                {
                                    animator.SetTrigger("Taunt");
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log(PlayerHealth);
                        if (PlayerHealth > 0)
                        {

                            // Set the destination to the player's position
                            agent.SetDestination(player.transform.position);
                        }
                    }
                }
                else
                {
                    // Stop playing the audio when the npc is not following the character
                    if (audioSource1.isPlaying)
                    {
                        audioSource1.Stop();
                    }
                    isMoving = false;

                }

                // Update the time since last attack
                timeSinceLastAttack += Time.deltaTime;
            }
        }
        if (hasEntered)
        {
            if (AreAllObjectsDestroyed())
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (Health > 0)
        {
            Health -= damage;
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

        }
        if(phase == 1)
        {
            
            if (Health < 7.5) 
            {
                
                invulnerable = true;
                invul();
            }
        }else if (phase == 2)
        {
 
            if (Health < 5)
            {
                
                invulnerable = true;
                invul();
            }
        }
        else if (phase == 3)
        {
            
            if (Health < 2.5)
            {
  
                invulnerable = true;
                invul();
            }
        }
        // If the NPC's health is depleted, trigger death sequence

        if (Health <= 0)
        {
            if (Alive == true)
            {
                Alive = false;
                isAggressive = false;
                isMoving = false;
                isAttacking = false;
                animator.SetBool("isMoving", isMoving);
                animator.SetTrigger("Die");
                agent.isStopped = true;
                audioSource1.Stop();
                audioSource2.clip = deathSound;
                audioSource2.Play();
                Destroy(gameObject, 5f);

            }
        }
        Debug.Log(Health);
    }

    private bool AreAllObjectsDestroyed()
    {
        return spawnedObject1 == null && spawnedObject2 == null && spawnedObject3 == null &&
               spawnedObject4 == null && spawnedObject5 == null && spawnedObject6 == null;
    }

    private void SpawnObject()
    {
        if (phase == 1)
        {
            spawnedObject1 = Instantiate(objectToSpawn1, spawnPoint1.position, spawnPoint1.rotation);
            spawnedObject2 = Instantiate(objectToSpawn1, spawnPoint2.position, spawnPoint2.rotation);
            spawnedObject3 = Instantiate(objectToSpawn1, spawnPoint3.position, spawnPoint3.rotation);
            spawnedObject4 = Instantiate(objectToSpawn2, spawnPoint4.position, spawnPoint4.rotation);
            spawnedObject5 = Instantiate(objectToSpawn2, spawnPoint5.position, spawnPoint5.rotation);
            spawnedObject6 = Instantiate(objectToSpawn2, spawnPoint6.position, spawnPoint6.rotation);
            newObject1 = Instantiate(prefabToInstantiateA, spawnPoint1.position, spawnPoint1.rotation);
            newObject2 = Instantiate(prefabToInstantiateA, spawnPoint2.position, spawnPoint2.rotation);
            newObject3 = Instantiate(prefabToInstantiateA, spawnPoint3.position, spawnPoint3.rotation);
            newObject4 = Instantiate(prefabToInstantiateA, spawnPoint4.position, spawnPoint4.rotation);
            newObject5 = Instantiate(prefabToInstantiateA, spawnPoint5.position, spawnPoint5.rotation);
            newObject6 = Instantiate(prefabToInstantiateA, spawnPoint6.position, spawnPoint6.rotation);
            Destroy(newObject1, 1f);
            Destroy(newObject2, 1f);
            Destroy(newObject3, 1f);
            Destroy(newObject4, 1f);
            Destroy(newObject5, 1f);
            Destroy(newObject6, 1f);
        }
        else if (phase == 2)
        {
                spawnedObject1 = Instantiate(objectToSpawn3, spawnPoint1.position, spawnPoint1.rotation);
                spawnedObject2 = Instantiate(objectToSpawn3, spawnPoint2.position, spawnPoint2.rotation);
                spawnedObject3 = Instantiate(objectToSpawn3, spawnPoint3.position, spawnPoint3.rotation);
                spawnedObject4 = Instantiate(objectToSpawn4, spawnPoint4.position, spawnPoint4.rotation);
                spawnedObject5 = Instantiate(objectToSpawn4, spawnPoint5.position, spawnPoint5.rotation);
                spawnedObject6 = Instantiate(objectToSpawn4, spawnPoint6.position, spawnPoint6.rotation);
            newObject1 = Instantiate(prefabToInstantiateA, spawnPoint1.position, spawnPoint1.rotation);
            newObject2 = Instantiate(prefabToInstantiateA, spawnPoint2.position, spawnPoint2.rotation);
            newObject3 = Instantiate(prefabToInstantiateA, spawnPoint3.position, spawnPoint3.rotation);
            newObject4 = Instantiate(prefabToInstantiateA, spawnPoint4.position, spawnPoint4.rotation);
            newObject5 = Instantiate(prefabToInstantiateA, spawnPoint5.position, spawnPoint5.rotation);
            newObject6 = Instantiate(prefabToInstantiateA, spawnPoint6.position, spawnPoint6.rotation);
            Destroy(newObject1, 1f);
            Destroy(newObject2, 1f);
            Destroy(newObject3, 1f);
            Destroy(newObject4, 1f);
            Destroy(newObject5, 1f);
            Destroy(newObject6, 1f);
        }
        else if (phase == 3)
        {
            spawnedObject1 = Instantiate(objectToSpawn5, spawnPoint1.position, spawnPoint1.rotation);
            spawnedObject2 = Instantiate(objectToSpawn5, spawnPoint2.position, spawnPoint2.rotation);
            spawnedObject3 = Instantiate(objectToSpawn5, spawnPoint3.position, spawnPoint3.rotation);
            spawnedObject4 = Instantiate(objectToSpawn6, spawnPoint4.position, spawnPoint4.rotation);
            spawnedObject5 = Instantiate(objectToSpawn6, spawnPoint5.position, spawnPoint5.rotation);
            spawnedObject6 = Instantiate(objectToSpawn6, spawnPoint6.position, spawnPoint6.rotation);
            newObject1 = Instantiate(prefabToInstantiateA, spawnPoint1.position, spawnPoint1.rotation);
            newObject2 = Instantiate(prefabToInstantiateA, spawnPoint2.position, spawnPoint2.rotation);
            newObject3 = Instantiate(prefabToInstantiateA, spawnPoint3.position, spawnPoint3.rotation);
            newObject4 = Instantiate(prefabToInstantiateA, spawnPoint4.position, spawnPoint4.rotation);
            newObject5 = Instantiate(prefabToInstantiateA, spawnPoint5.position, spawnPoint5.rotation);
            newObject6 = Instantiate(prefabToInstantiateA, spawnPoint6.position, spawnPoint6.rotation);
            Destroy(newObject1, 1f);
            Destroy(newObject2, 1f);
            Destroy(newObject3, 1f);
            Destroy(newObject4, 1f);
            Destroy(newObject5, 1f);
            Destroy(newObject6, 1f);
        }
        phase++;
    }
    private void invul()
    {
        // Stop the NPC's movement
        agent.isStopped = false;

        // Set the destination to the NPC's initial spawn position
        agent.SetDestination(initialPosition);

        // Reset other NPC variables as needed
        isAggressive = false;
        isMoving = true; // Set isMoving to true to trigger walking animation
        isAttacking = false;
        animator.SetBool("isMoving", isMoving);
        SpawnObject();
        hasEntered= true;
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
        if (other.tag == "ElectricSlash" && !sideTrigger)
        {

            sideTrigger = true;
            Debug.Log("im electrecuted");
            shock();
            TakeDamage(0.2f);


        }
        if (other.tag == "ElectricCircle" && !ultTrigger)
        {

            sideTrigger = true;
            Debug.Log("im electrecuted");
            electrify();
            TakeDamage(0.3f);


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
    //electric methods
    void shock()
    {
        electrified = true;
        Invoke("notElectrified", 5f); // Restore trigger
        Invoke("restoreSide", 6f); // Restore trigger

    }
    void electrify()
    {
        electrified = true;
        Invoke("notElectrified", 8f); // Restore trigger
        Invoke("restoreUlt", 12f); // Restore trigger

    }
    void notElectrified()
    {
        electrified = false;
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
            Invoke("restoreSide", 6f); // Restore trigger
        }
        if (Fire == 2)
        {
            evaporate();
            Invoke("restoreUlt", 12f); // Restore trigger
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
            Invoke("restoreSide", 6f); // Restore agent speed after 4 seconds
        }
        if (Ice == 2)
        {
            Invoke("restoreUlt", 12f); // Restore agent speed after 4 seconds
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





        // Inflict damage to player
        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            if (electrified)
            {

                GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Enemy");
                if (countOfNpc > 1)
                {
                    float closestDistance = Mathf.Infinity;
                    GameObject closestNPC = null;
                    Vector3 sourcePosition = transform.position; // Assuming this script is attached to the source object

                    foreach (GameObject npcObject in npcObjects)
                    {
                        if (npcObject != gameObject) // Exclude the source object itself from consideration
                        {
                            Vector3 npcPosition = npcObject.transform.position;
                            float distance = Vector3.Distance(sourcePosition, npcPosition);

                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestNPC = npcObject;
                            }
                        }
                    }

                    // Use the 'closestNPC' object for further interactions or operations
                    if (closestNPC != null)
                    {
                        player = closestNPC;
                    }
                    NPCMovement npcmovement = player.GetComponent<NPCMovement>();

                    audioSource2.clip = attackSound;
                    audioSource2.Play();
                    //float dmg = Random.Range(0.3f, 0.5f);
                    //float roundedDamage = Mathf.Round(dmg * 100f) / 100f; // round to two decimal places
                    npcmovement.TakeDamage(DamageDone);
                }

            }
            else
            {

                GameObject character = GameObject.FindWithTag("Player"); //Jack prefab is tagged as player
                if (character != null)
                {
                    CharacterMovement characterMovement = character.GetComponent<CharacterMovement>();
                    if (!characterMovement.isDefending)
                    {
                        audioSource2.clip = attackSound;
                        audioSource2.Play();
                        characterMovement.animator.SetTrigger("isHit");
                    }
                    else
                    {
                        Debug.Log("is defending");
                        audioSource2.clip = defendedSound;
                        audioSource2.Play();
                        characterMovement.animator.SetTrigger("isHitDefending");
                    }
                    //float dmg = Random.Range(0.3f, 0.5f);
                    //float roundedDamage = Mathf.Round(dmg * 100f) / 100f; // round to two decimal places
                    characterMovement.TakeDamage(DamageDone);
                }
            }

            yield return new WaitForSeconds(delay);

            // Resume following the player
            agent.isStopped = false;
        }
    }



}