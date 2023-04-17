using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscordMessageText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text messageText;

    private RectTransform messageRect;
    public RectTransform MessageRect
    {
        get
        {
            messageRect ??= GetComponent<RectTransform>();
            return messageRect;
        }
    }

    public void SettingMessage(string messsage)
    {
        messageText.text = messsage;
        AutoContectSize();
    }

    private void AutoContectSize()
    {
        Vector2 size = messageText.rectTransform.sizeDelta;
        bool isLine = false;
        for(int i = 0; i < messageText.text.Length; i++)
        {
            if(messageText.text[i] == '\n')
            {
                isLine = true;
                size.y += 20;
            }
        }
        if(isLine)
        {
            size.y += 10;
        }
        messageText.rectTransform.sizeDelta = size;
    }

    public void SetColor(Color color)
    {
        messageText.color = color;
    }
}
