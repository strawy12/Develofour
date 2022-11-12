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

    private int cnt = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent(EEvent.OpenTextBox, ETextDataType.News);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            NoticeData data = new NoticeData();
            data.head = "�̰��� �׽�Ʈ�Դϴ�";
            data.body = "�׽�Ʈ�ϱ� �� �������� Ȯ���ϰ�\n���� ���ڵ� �Ǵ��� Ȯ���ϰڽ��ϴ�.";
            NoticeSystem.OnGeneratedNotice?.Invoke(data);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sound.OnPlayEffectSound(Sound.EEffect.NewsAnchorVoice_01 + (cnt++));
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
