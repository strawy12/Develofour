using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    public Action OnChangedValue;
    private void Awake()
    {
        Init();
    }

    public abstract void Init();
    public abstract bool CheckDecision();
}
