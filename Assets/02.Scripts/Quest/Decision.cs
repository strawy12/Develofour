using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    public Action OnChangedValue;
    public bool isClear;

    public virtual void SettingClear(bool clear)
    {
        isClear = clear;
    }

    public abstract void Init();
    public virtual bool CheckDecision()
    {
        return isClear;
    }
}
