using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    public string decisionName;

    public Action OnChangedValue;
    public Action<Decision> OnClearPanel;
    public bool isClear;

    public abstract void Init();

    public virtual bool CheckDecision()
    {
        OnChangedValue.Invoke();
        OnClearPanel.Invoke(this);
        return isClear;
    }
}
