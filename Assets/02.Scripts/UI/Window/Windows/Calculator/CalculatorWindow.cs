using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorWindow : Window
{
    [SerializeField]
    private List<AntilogarithmPanel> antilogarithmPanelList;

    [SerializeField]
    private List<CalculatorClick> buttonList;

    protected override void Init()
    {
        base.Init();
    }
}
