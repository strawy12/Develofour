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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TargetPanels.OnClosed();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TargetPanels.OnOpened(null);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sound.OnPlayBGMSound(Sound.EBgm.WriterBGM);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            NoticeData data = new NoticeData();
            data.head = "이것은 테스트입니다";
            data.body = "테스트니까 잘 나오는지 확인하고\n개행 문자도 되는지 확인하겠습니다.";
            NoticeSystem.OnGeneratedNotice?.Invoke(data);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Sound.OnPlayBGMSound(Sound.EBgm.WriterBGM);
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
