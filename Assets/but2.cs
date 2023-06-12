using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class but2 : MonoBehaviour
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
    public void ChangeUlt(int ult)
    {
        aoe.UltAbility = ult;
        Debug.Log(ult);
    }
}
