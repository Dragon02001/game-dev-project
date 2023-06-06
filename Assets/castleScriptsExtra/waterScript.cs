using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScript : MonoBehaviour
{
    public LevelManager levelManager;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Invoke("RestartLevel", 2f);

        }
    }
    private void RestartLevel()
    {
        levelManager.RestartLevel();
    }
}
