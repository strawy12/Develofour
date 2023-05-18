using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowLoginTimePanel : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text dateText;
    private DateTime dateTime;

    string hourText;
    string minuteText;

    void Start()
    {
        EventManager.StartListening(ETimeEvent.ChangeTime, SetDateTime);
    }

    public void SetDateTime(object[] ps)
    {
        if (!(ps[0] is System.DateTime))
        {
            return;
        }

        dateTime = (System.DateTime)ps[0];

        if (dateTime.Hour < 10)
        {
            hourText = "0" + dateTime.Hour.ToString();
        }
        else
        {
            hourText = dateTime.Hour.ToString();
        }

        if (dateTime.Minute < 10)
        {
            minuteText = "0" + dateTime.Minute.ToString();
        }
        else
        {
            minuteText = "0".ToString();
        }

        SetText();
    }

    private void SetText()
    {
        timeText.text = hourText + ":" + minuteText;
        dateText.text = dateTime.Month + "월 " + dateTime.Day + "일 " + GetDayText(dateTime.DayOfWeek);
    }

    private string GetDayText(DayOfWeek week)
    {
        switch(week)
        {
            case DayOfWeek.Monday:
                return "월요일";
            case DayOfWeek.Tuesday:
                return "화요일";
            case DayOfWeek.Wednesday:
                return "수요일";
            case DayOfWeek.Thursday:
                return "목요일";
            case DayOfWeek.Friday:
                return "금요일";
            case DayOfWeek.Saturday:
                return "토요일";
            case DayOfWeek.Sunday:
                return "일요일";
        }
        return null;
    }
}
