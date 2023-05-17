using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagepopup : MonoBehaviour
{

    public static damagepopup current;
    public GameObject prefab;
    // Start is called before the first frame update
     void Awake()
    {
        current= this;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update called");
        if (Input.GetKeyDown(KeyCode.F10)) 
        {
            // Get a reference to the character game object
            GameObject character = GameObject.FindGameObjectWithTag("Enemy");

            // Calculate a position near the character
            Vector3 offset = new Vector3(0.0f, 2.5f, 1.0f); // Vertical offset from the character
            Vector3 position = character.transform.position + offset;
            Debug.Log(position);
            // Create the pop-up at the calculated position
            CreatePopUp(position, Random.Range(0, 1000).ToString(), Color.yellow);
        }
    }
    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        // Rotate the pop-up to face the camera
        popup.transform.LookAt(Camera.main.transform);
        popup.transform.Rotate(new Vector3(0, 180, 0));

        // Destroy the pop-up after 2 seconds
        Destroy(popup, 1f);
    }

}
