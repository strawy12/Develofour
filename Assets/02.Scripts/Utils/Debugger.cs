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
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    FileManager.Inst.AddFile(todoFile, "C/Background");
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.Default });
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        }
    }

}
