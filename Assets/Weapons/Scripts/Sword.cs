using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
      //  Enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {


    }

    public CharacterMovement cm;
    // public GameObject HitParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && cm.isAttacking)
        {
            Debug.Log(other.name);
            NPCMovement enemy = other.GetComponent<NPCMovement>(); //Retrieve the NPCMovement component from the GameObject
            if (enemy != null)
            {
                float damage = Random.Range(0f, 0.5f);
                float roundedDamage = Mathf.Round(damage * 100f) / 100f; // round to two decimal places
                enemy.TakeDamage(roundedDamage);
            }



            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.poistion.y, other.transform.position.z), other.transform.rotation);
        }
    }
}