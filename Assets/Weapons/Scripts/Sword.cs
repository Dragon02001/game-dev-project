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
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //  Enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && cm.playerStamina > 0.0f)
        {
            cm.isAttacking = true;
            //Debug.Log(comboCount);
            //Debug.Log(cm.isAttacking);
            // Start the appropriate attack animation based on the current combo count
            if (comboCount == 0 && cm.playerStamina > 0.1f)
            {
                cm.animator.SetTrigger("combo1");
                comboCount++;
                lastComboAttackTime = Time.time;
                cm.playerStamina -= 0.1f;
                audioSource.PlayOneShot(attackSound);


            }
            else if (comboCount == 1 && Time.time - lastComboAttackTime < 1f && cm.playerStamina > 0.1f)
            {
                cm.animator.SetTrigger("combo2");
                comboCount++;
                lastComboAttackTime = Time.time;
                cm.playerStamina -= 0.1f;
                audioSource.PlayOneShot(attackSound);

            }
            else if (comboCount == 2 && cm.playerStamina > 0.3f)
            {
                cm.animator.SetTrigger("combo3");
                cm.playerStamina -= 0.3f;
                audioSource.PlayOneShot(SpinningAttackSound);
            }
            else
            {
                stamina.pulse();
                cm.animator.SetBool("isDizzy", true);
            }

            // Play attack sound and decrement player stamina
            
            


            // If the combo count has exceeded the maximum combo count, reset the combo count
            if (comboCount > maxComboCount)
            {
                comboCount = 0;
            }

            lastAttackTime = Time.time;
        }
        IEnumerator WaitAndSetAttacking(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            cm.isAttacking = false;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            cm.isAttacking = false;
           // StartCoroutine(WaitAndSetAttacking(0.5f)); // Wait for 0.5 seconds before setting isAttacking to false
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

        if (other.tag == "Enemy" && cm.isAttacking)
        {
            Debug.Log(other.name);
            NPCMovement enemy = other.GetComponent<NPCMovement>(); //Retrieve the NPCMovement component from the GameObject
            if (enemy != null)
            {
                float damage;
                if (comboCount == 2)
                {
                    damage = Random.Range(0.5f, 0.8f);
                }
                else
                {
                    damage = Random.Range(0.3f, 0.5f);
                }
                float roundedDamage = Mathf.Round(damage * 100f) / 100f; // round to two decimal places
                enemy.TakeDamage(roundedDamage);
            }



            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.poistion.y, other.transform.position.z), other.transform.rotation);
        }
    }
}