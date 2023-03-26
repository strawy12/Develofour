using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPadInput : Button
{
    public Action<string> OnClick;

    public string currentData;

    public void ClickBtn()
    {
        OnClick?.Invoke(currentData);
    }


    [ContextMenu("SetCurrentData")]
    public void SetCurrentData()
    {
        currentData = gameObject.name;

    }
}
