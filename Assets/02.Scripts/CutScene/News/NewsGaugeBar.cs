using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NewsGaugeBar : MonoBehaviour
{
    public float gaugeSpeed;

    private float currentValue;

    [SerializeField]
    private Image circleGauge;
    [SerializeField]
    private TMP_Text percentText;


    private void Start()
    {
        currentValue = 0;

        InputManager.Inst.AddKeyInput(KeyCode.Space, onKeyStay: OnKeyDownSkip, onKeyUp: OnKeyUpSkip);
    }

    // �ٿ� ���� �� �� �ƴ� �ٿ� �ϴ� �� �� �� ����
    private void OnKeyDownSkip()
    {
        if (currentValue >= 100)
        {
            Skip();
        }
        else
        {
            currentValue += gaugeSpeed * Time.deltaTime;
            SetPercentText();
        }

        circleGauge.fillAmount = currentValue / 100;
    }

    private void OnKeyUpSkip()
    {
        if (currentValue < 100)
        {
            currentValue = 0;
            SetPercentText();
        }

        circleGauge.fillAmount = currentValue / 100;
    }
    
    private void Skip()
    {
        EventManager.TriggerEvent(ECutSceneEvent.SkipCutScene);

        InputManager.Inst.RemoveKeyInput(KeyCode.Space, onKeyStay: OnKeyDownSkip, onKeyUp: OnKeyDownSkip);
    }

    private void SetPercentText()
    {
        percentText.SetText(((int)currentValue).ToString() + '%');
    }
}
