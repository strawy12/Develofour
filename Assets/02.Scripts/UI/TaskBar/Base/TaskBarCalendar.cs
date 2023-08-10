using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public enum EMeridiems
{
    AM,
    PM
}

public class TaskBarCalendar : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text dayText;
    private System.DateTime dateTime;
    private string meridiemText;
    private EMeridiems meridiem;
    private int hour;
    string hourText;
    string minuteText;

    void Start()
    {
        EventManager.StartListening(ETimeEvent.ChangeTime, SetDateTime);
    }

    public void SetDateTime(object[] ps)
    {
        if(!(ps[0] is System.DateTime))
        {
            return;
        }

        dateTime = (System.DateTime)ps[0];

        if (dateTime.Hour > 12)
        {
            hour = dateTime.Hour - 12;
            meridiem = EMeridiems.PM;
        }
        else
        {
            hour = dateTime.Hour;
            meridiem = EMeridiems.AM;
        }

        meridiemText = meridiem == EMeridiems.AM ? "오전" : "오후";

        if (hour < 10)
        {
            hourText = "0" + hour.ToString();
        }
        else
        {
            hourText = hour.ToString();
        }

        if (dateTime.Minute < 10)
        {
            minuteText = "0" + dateTime.Minute.ToString();
        }
        else
        {
            minuteText = dateTime.Minute.ToString();
        }
        SetText();
    }
    public void SetText()
    {
        timeText.text = $"{meridiemText}  {hourText}:{minuteText}";
        dayText.text = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";
    }

}
