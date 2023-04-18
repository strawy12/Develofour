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
            time = $"오후 {timeData.time - 12}:{timeData.minute}";
        }
        else
        {
            time = $"오전 {timeData.time}:{timeData.minute}";
        }

        if (timeData.year == Constant.NOWYEAR && Define.CheckTodayDate(timeData.day))
        {
            text = $"오늘 {time}";
        }
        else if(timeData.year == Constant.NOWYEAR  && Define.CheckYesterDayDate(timeData.day))
        {
            text = $"어제 {time}";
        }
        else
        {
            text = $"{timeData.year}.{timeData.month}.{timeData.day} {time}";
        }
        timeText.SetText(text);

        gameObject.SetActive(false);
    }
}
