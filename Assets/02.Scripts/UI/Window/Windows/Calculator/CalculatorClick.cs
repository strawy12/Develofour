using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorClick : MonoBehaviour
{
    public Button button;
    public string value;

    public CalculatorWindow calculator;

    public void Init()
    {
        button.onClick?.AddListener(OnClick);
    }

    public void OnClick()
    {
        calculator.GetClicked(value);
    }
}
