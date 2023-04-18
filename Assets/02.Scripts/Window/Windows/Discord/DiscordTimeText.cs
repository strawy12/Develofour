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
        string time = "";

        if (timeData.time >= 12)
        {
            time = $"���� {timeData.time - 12}:{timeData.minute}";
        }
        else
        {
            time = $"���� {timeData.time}:{timeData.minute}";
        }

        if (timeData.year == Constant.NOWYEAR && Define.CheckTodayDate(timeData.day))
        {
            text = $"���� {time}";
        }
        else if(timeData.year == Constant.NOWYEAR  && Define.CheckYesterDayDate(timeData.day))
        {
            text = $"���� {time}";
        }
        else
        {
            text = $"{timeData.year}.{timeData.month}.{timeData.day} {time}";
        }
        timeText.SetText(text);

        gameObject.SetActive(false);
    }
}
