using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class but : MonoBehaviour
{
   
    [SerializeField]
    public AOE aoe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSide(int side)
    {
        aoe.SideAbility = side;
        Debug.Log(side);
    }
}
