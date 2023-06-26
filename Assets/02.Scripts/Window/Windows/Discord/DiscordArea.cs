using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscordArea : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text inputText;

    private string inputString = "에 메세지 보내기";

    public Action OnAttributePanelOff;

    public void SettingInputText(string name)
    {
        inputText.text = "#" + name + inputString;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnAttributePanelOff?.Invoke();
        }
    }
}
