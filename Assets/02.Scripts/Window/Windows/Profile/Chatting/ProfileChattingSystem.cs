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
        EventManager.StartListening(EProfileEvent.SendGuide, AddGuide);

        EventManager.StartListening(ETextboxEvent.Delay, SetDelay);
        EventManager.StartListening(EDebugSkipEvent.TutorialSkip, delegate { isSkip = true; });
    }

    public void AddText(object[] ps)
    {
        if (ps[0] is EAIChattingTextDataType)
        {
            EAIChattingTextDataType chattingType = (EAIChattingTextDataType)ps[0];
            StartCoroutine(AddText((chattingType)));
            return;
        }
       
        Debug.LogError("형식이 잘못되었습니다.");
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
        OnChatEnd = null;
    }

    public void AddText(string str)
    {
        foreach (var save in saveData.saveList)
        {
            if (save == str)
            {
                Debug.Log("동일한 데이터");
                return;
            }
        }

        NoticeSystem.OnNotice.Invoke("AI에게서 메세지가 도착했습니다!", str, 0, true, null, Color.white, ENoticeTag.AIAlarm);

        saveData.saveList.Add(str);
        EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { str });
    }

    public void SetDelay(object[] ps)
    {
        if (ps[0] is float)
        {
            currentDelay = (float)ps[0];
        }
    }
    #region Guide
    public void AddGuide(object[] ps)
    {
        if (ps[0] is EAIChattingTextDataType)
        {
            EAIChattingTextDataType chattingType = (EAIChattingTextDataType)ps[0];
            StartCoroutine(AddGuide((chattingType)));
            return;
        }
        Debug.LogError("형식이 잘못되었습니다.");
    }
    public IEnumerator AddGuide(EAIChattingTextDataType type)
    {
        AIChattingTextDataSO data = ResourceManager.Inst.GetAIChattingTextDataSO(type);

        for (int i = 0; i < data.Count; i++)
        {
            TextTrigger.CommandTrigger(data[i].text);
            yield return new WaitForSeconds(0.5f);
            string text = TextTrigger.RemoveCommandText(data[i].text, null, null);
            AddGuide(text);
        }

        OnChatEnd?.Invoke();
        OnChatEnd = null;
    }
    public void AddGuide(string str)
    {
        NoticeSystem.OnNotice.Invoke("AI에게서 메세지가 도착했습니다!", str, 0, true, null, Color.white, ENoticeTag.AIAlarm);

        saveData.saveList.Add(str);
        EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { str });
    }
    #endregion
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        saveData.Reset();
    }
#endif
}
