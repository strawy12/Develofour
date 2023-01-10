using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class TaskBarCalendar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum EMeridiems
    {
        AM,
        PM
    }
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text dayText;
    [SerializeField]
    private Image highlightImage;
    private DateTime dateTime;
    private string meridiemText;

    public void SetDateTime(int year, int month, int day, int hour, int minute, int second, EMeridiems meridiem = EMeridiems.PM)
    {
        if (hour > 12)
        {
            hour = hour - 12;
            meridiem = EMeridiems.PM;
        }
        dateTime = new DateTime(year, month, day, hour, minute, second);
        meridiemText = meridiem == EMeridiems.AM ? "오전" : "오후";
        SetText();
    }
    public void SetText()
    {
        timeText.text = $"{meridiemText}  {dateTime.Hour}:{dateTime.Minute}";
        dayText.text = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Date}";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightImage.gameObject.SetActive(true);
    }
}
