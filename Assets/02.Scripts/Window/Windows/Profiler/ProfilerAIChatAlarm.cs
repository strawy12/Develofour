using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfilerAIChatAlarm : MonoBehaviour
{
    public GameObject background;
    public TMP_Text alarmText;

    public ProfilerChatting aiChatPanel;

    private int alarmCount = 0;

    public void Init()
    {
        EventManager.StartListening(EWindowEvent.AlarmSend, AlarmCheck);
        CloseAlarm();
    }

    public void CloseAlarm()
    {
        alarmCount = 0;
        background.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void AlarmCheck(object[] ps)
    {
        if (!(ps[0] is EWindowType))
        {
            return;
        }

        if(aiChatPanel.isFlag == true)
        {
            return;
        }

        EWindowType type = (EWindowType)ps[0];

        if (type != EWindowType.ProfilerWindow)
        {
            return;
        }

        alarmCount += 1;

        alarmText.text = alarmCount.ToString();
        background.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }
}
