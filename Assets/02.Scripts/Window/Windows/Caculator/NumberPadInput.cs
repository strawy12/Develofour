using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPadInput : Button
{
    public Action<string> OnClick;

    public string currentData;

    protected override void Start()
    {
        onClick?.AddListener(SetCurrentData);
    }

    public void ClickBtn()
    {
        OnClick?.Invoke(currentData);
    }


    public void SetCurrentData()
    {
        currentData = gameObject.name;
        ClickBtn();
    }
}
