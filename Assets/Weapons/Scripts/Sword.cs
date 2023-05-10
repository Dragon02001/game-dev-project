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
                enemy.TakeDamage(0.4f); //Call the TakeDamage function on the NPCMovement component
            }
            //  other.GetComponent<Animator>().SetBool("isAttacking", false);
            // other.GetComponent<Animator>().SetTrigger("Hit");

            //  other.GetComponent<Animator>().ResetTrigger("Hit");

            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.poistion.y, other.transform.position.z), other.transform.rotation);
        }
    }
}