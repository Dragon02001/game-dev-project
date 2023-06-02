using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public CharacterMovement cm;
    private float lastComboAttackTime = 0.0f;
    private int comboCount = 0;
    private int maxComboCount = 2;
    private float lastAttackTime = 0.0f;
    public AudioClip attackSound;
    public AudioClip SpinningAttackSound;
    private AudioSource audioSource;
    public Stamina stamina;
    private bool highDamage = false;
    float damage;
    public float attackCooldown = 0.5f; // Cooldown period between attacks
    private bool canAttack = true; // Flag to track if the player can perform an attack

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && Input.GetMouseButtonDown(0) && cm.playerStamina > 0.0f)
        {
            cm.isAttacking = true;

            if (comboCount == 0 )
            {
                cm.animator.SetTrigger("combo1");
                comboCount++;
                lastComboAttackTime = Time.time;
               
                audioSource.PlayOneShot(attackSound);
                damage = Random.Range(0.1f, 0.3f);

            }
            else if (comboCount == 1 && Time.time - lastComboAttackTime < 1f )
            {
                cm.animator.SetTrigger("combo2");
                comboCount++;
                lastComboAttackTime = Time.time;
               
                audioSource.PlayOneShot(attackSound);
                damage = Random.Range(0.3f, 0.5f);

            }
            else if (comboCount == 2 && cm.playerStamina > 0.1f)
            {
                cm.animator.SetTrigger("combo3");
                
                audioSource.PlayOneShot(SpinningAttackSound);
                highDamage = true;
                damage = Random.Range(0.5f, 0.8f);
            }
            else
            {
                stamina.pulse();
                cm.animator.SetBool("isDizzy", true);
            }

            // Play attack sound and decrement player stamina

            if (comboCount > maxComboCount)
            {
                comboCount = 0;
            }

            lastAttackTime = Time.time;
            canAttack = false; // Disable further attacks

            StartCoroutine(EnableAttackAfterCooldown()); // Start the cooldown timer
        }

        IEnumerator EnableAttackAfterCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true; // Enable attacks after the cooldown period
        }
    
        IEnumerator WaitAndSetAttacking(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            cm.isAttacking = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //cm.isAttacking = false;
            StartCoroutine(WaitAndSetAttacking(0.4f)); // Wait for 0.5 seconds before setting isAttacking to false
            audioSource.Stop();
        }

        // If the combo timer has elapsed, reset the combo count
        if (Time.time - lastComboAttackTime > 1f)
        {
            comboCount = 0;
        }

    }

    
    // public GameObject HitParticle;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && cm.isAttacking)
        {
            Debug.Log(other.name);

            // Check for different enemy scripts using their specific tags or components
            if (other.GetComponent<NPCMovement>() != null)
            {
                NPCMovement npcEnemy = other.GetComponent<NPCMovement>();
                float roundedDamage = Mathf.Round(damage * 100f) / 100f; // Round to two decimal places
                npcEnemy.TakeDamage(roundedDamage);
            }
            else if (other.GetComponent<skeletonGaurd>() != null)
            {
                skeletonGaurd npcEnemy = other.GetComponent<skeletonGaurd>();
                float roundedDamage = Mathf.Round(damage * 100f) / 100f; // Round to two decimal places
                npcEnemy.TakeDamage(roundedDamage);

            }
            //else if (other.GetComponent<EnemyScript2>() != null)
            //{
            //    EnemyScript2 enemyScript2 = other.GetComponent<EnemyScript2>();
            //    // Handle enemy script 2 specific logic
            //}

            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.poistion.y, other.transform.position.z), other.transform.rotation);
        }
    }
}