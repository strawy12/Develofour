using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalculatorWindow : Window
{
    [Header("CalculatorUI")]
    [SerializeField]
    private TMP_Text numberCalculatorText;
    [SerializeField]
    private Button eraseButton;

    private Queue<int> calculatorQueue;

    private string currentCalcualtorData;

    protected override void Init()
    {
        base.Init();

        GetButtonAction();
    }

    private void GetButtonAction()
    {
        eraseButton.onClick?.AddListener(EraseButton);

        NumberPadInput[] btns = GetComponentsInChildren<NumberPadInput>();

        foreach (NumberPadInput btn in btns)
        {
            btn.OnClick += OnClickBtn;
        }
    }

    private void OnClickBtn(string data)
    {
        int dataInt = Int32.Parse(data);

        switch (data)
        {
            case "1":
            case "2":
            case "3":
            case "4":
            case "5":
            case "6":
            case "7":
            case "8":
            case "9":
            case "10":
                calculatorQueue.Enqueue(dataInt);

                break;


        }

        NumberTextMark(data);
        

    }

    private void EraseButton()
    {

    }

    private void NumberTextMark(string data)
    {
        currentCalcualtorData += data;

        string comaAdditionStr = string.Format("{0:#,###}", currentCalcualtorData);

        numberCalculatorText.SetText(comaAdditionStr);
    }

}
