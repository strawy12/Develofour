using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileChattingSystem : MonoBehaviour
{
    [SerializeField]
    private ProfileChattingSaveSO saveData;

    private void Awake()
    {
        EventManager.StartListening(EProfileEvent.SaveMessage, AddText);
    }

    public void AddText(object[] ps)
    {
        if (!(ps[0] is EAiChatData))
        {
            if (ps[0] is string)
            {
                AddText(ps[0] as string);
            }
            return;
        }

        EAiChatData data = (EAiChatData)ps[0];

        foreach (var save in saveData.saveList)
        {
            if (save == data.ToString())
            {
                Debug.Log("������ ������");
                return;
            }
        }
        saveData.saveList.Add(data.ToString());
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { data });
    }

    public void AddText(string str)
    {
        foreach (var save in saveData.saveList)
        {
            if (save == str)
            {
                Debug.Log("������ ������");
                return;
            }
        }

        NoticeSystem.OnNotice.Invoke("AI���Լ� �޼����� �����߽��ϴ�!", str, 0, true, null, ENoticeTag.AIAlarm);

        saveData.saveList.Add(str);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
    }
}
