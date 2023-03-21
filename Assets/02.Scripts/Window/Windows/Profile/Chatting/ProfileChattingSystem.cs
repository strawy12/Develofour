using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileChattingSystem : MonoBehaviour
{
    [SerializeField]
    private ProfileChattingSaveSO saveData;

    private float currentDelay;
    private float saveDelay = 2f;
    private bool isSkip;
    public static Action OnChatEnd;

    private void Awake()
    {
        EventManager.StartListening(EProfileEvent.SendMessage, AddText);
        EventManager.StartListening(ETextboxEvent.Delay, SetDelay);
        EventManager.StartListening(EDebugSkipEvent.TutorialSkip, delegate { isSkip = true; });
    }

    public void AddText(object[] ps)
    {
        if (ps[0] is EAIChattingTextDataType)
        {
            if (ps[0] is EAIChattingTextDataType)
            {
                StartCoroutine(AddText((EAIChattingTextDataType)ps[0]));
            }
            return;
        }
        Debug.LogError("������ �߸��Ǿ����ϴ�.");
    }

    public IEnumerator AddText(EAIChattingTextDataType type)
    {
        AIChattingTextDataSO data = ResourceManager.Inst.GetAIChattingTextDataSO(type);

        for (int i = 0; i < data.Count; i++)
        {
            
            currentDelay = saveDelay;
            TextTrigger.CommandTrigger(data[i].text);
            if (isSkip) currentDelay = 0.05f;
            yield return new WaitForSeconds(currentDelay);
            string text = TextTrigger.RemoveCommandText(data[i].text, null, null);
            AddText(text);
        }

        OnChatEnd?.Invoke();
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

        NoticeSystem.OnNotice.Invoke("AI���Լ� �޼����� �����߽��ϴ�!", str, 0, true, null,Color.white, ENoticeTag.AIAlarm);

        saveData.saveList.Add(str);
        EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { str });
    }

    public void SetDelay(object[] ps)
    {
        if (ps[0] is float)
        {
            currentDelay = (float)ps[0];
            Debug.Log(currentDelay);
        }
    }

}
