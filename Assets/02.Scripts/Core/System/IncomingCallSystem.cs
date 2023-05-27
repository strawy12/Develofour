using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingCallSystem : MonoBehaviour
{
    

    void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }

    public void Init()
    {
       
    }

    
}
