using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscordTimeText : MonoBehaviour
{
    private TMP_Text timeText;

    public string TimeText
    {
        get
        {
            return timeText.text;
        }
    }

    public void Init()
    {
        timeText ??= GetComponent<TMP_Text>();

        timeText.SetText("");
    }
    public void SettingText(DiscordSendTime timeData)
    {
        timeText.SetText("");
        string text = "";
        text = $"{timeData.year}.{timeData.month}.{timeData.day}";
        timeText.SetText(text);

        gameObject.SetActive(false);
    }
}
