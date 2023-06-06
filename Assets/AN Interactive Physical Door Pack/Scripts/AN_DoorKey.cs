using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_DoorKey : MonoBehaviour
{
    [Tooltip("True - red key object, false - blue key")]
    public bool isRedKey = true;
    AN_HeroInteractive hero;

    // NearView()
    float distance;
    float angleView;
    Vector3 direction;

    private void Start()
    {
        hero = FindObjectOfType<AN_HeroInteractive>(); // key will get up and it will saved in "inventary"
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F) && NearView(3f, 90f))
        {
            if (isRedKey) hero.RedKey = true;
            else hero.BlueKey = true;
            Destroy(gameObject);
        }
    }

    bool NearView(float maxDistance, float maxAngle)
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogWarning("Player object not found with tag 'Player'. Make sure the player object has the correct tag assigned.");
            return false;
        }

        distance = Vector3.Distance(transform.position, playerObject.transform.position);
        direction = transform.position - playerObject.transform.position;
        angleView = Vector3.Angle(playerObject.transform.forward, direction);

        if (angleView < maxAngle && distance < maxDistance)
        {
            return true;

        }
        else
        {
            return false;

        }

    }
}
