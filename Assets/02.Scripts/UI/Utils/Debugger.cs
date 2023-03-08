using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DebugEvent
{
    public KeyCode keyCode;
    public UnityEvent debugEvent;
}

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private List<DebugEvent> debugEventList;

    [SerializeField]
    private FileSO todoFile;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSiteAll);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MonologSystem.OnStopMonolog?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }
    }

}
