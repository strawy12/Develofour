using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorWindow : Window
{
    [SerializeField]
    private List<AntilogarithmPanel> antilogarithmPanelList;

    [SerializeField]
    private List<CalculatorClick> buttonList;

    private string currentStr;

    protected override void Init()
    {
        base.Init();
        foreach(var temp in antilogarithmPanelList)
        {
            temp.Init();
        }
        foreach(var temp in buttonList)
        {
            temp.Init();
        }
    }

    public void GetClicked(string str)
    {
        currentStr += str;
    }
}
