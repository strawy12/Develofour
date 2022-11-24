using System.Collections;
using System.Collections.Generic;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Browser.OnOpenSite(ESiteLink.Email);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            NoticeData data = new NoticeData();
            data.head = "�̰��� �׽�Ʈ�Դϴ�";
            data.body = "�׽�Ʈ�ϱ� �� �������� Ȯ���ϰ�\n���� ���ڵ� �Ǵ��� Ȯ���ϰڽ��ϴ�.";
            NoticeSystem.OnGeneratedNotice?.Invoke(data);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EventManager.TriggerEvent(ECutSceneEvent.SkipCutScene);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sound.OnPlayBGMSound?.Invoke(Sound.EBgm.WriterBGM);
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
