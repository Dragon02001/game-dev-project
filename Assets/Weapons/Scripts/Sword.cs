using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
            other.GetComponent<Animator>().SetTrigger("Hit");

            // Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.poistion.y, other.transform.position.z), other.transform.rotation);
        }
    }
}