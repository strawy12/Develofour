using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GuideStatusData
{
    public bool isCompleted;
    public string guideTopic;
}

public class GuideManager : MonoSingleton<GuideManager>
{
    public bool isZooglePinNotePadOpenCheck = false;
    
    public Dictionary<string, bool> guidesDictionary;
    
    [SerializeField]
    private List<GuideStatusData> guideStatusesList;

    private string guideType;
    
    public string[] pinNotPadHintChatting;

    void Start()
    {
        guidesDictionary = new Dictionary<string, bool>();

        Init();
    }

    private void Init()
    {
        EventManager.StartListening(ECoreEvent.OpenPlayGuide, OnPlayGuide);

        DictionaryInit();
    }

    private void DictionaryInit()
    {
        foreach (GuideStatusData guideStatus in guideStatusesList)
        {
            guidesDictionary.Add(guideStatus.guideTopic, guideStatus.isCompleted);
        }
    }    

    private void OnPlayGuide(object[] ps)
    {
        if (ps[0] == null || ps[1] == null)
        {
            return;
        }

        float timer = (float)ps[0];
        guideType = ps[1].ToString();

        StartCoroutine(SetTimer(timer));
    }

    private IEnumerator SetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (!guidesDictionary[guideType]) // 완료되어 있지 않다면
        {
            StartGudie(guideType);
        }
    }

    private void StartGudie(string guideTopic)
    {
        switch(guideTopic)
        {
            case "ProfilerDownGuide":
                {
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuideLog1, 0.2f, 1);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case "BrowserConnectGuide":
                {
                    string str = "만약 지금 무엇을 하실지 모르겠다면, 주글 메일 사이트을 먼저 접속하시는 것을 추천합니다.";
                    
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;
                    
                    break;
                }
            case "ClickPinNotePadHint":
                {
                    string str = "같은 폴더 내의 Pin번호라는 파일이 존재합니다. 이 파일을 먼저 확인 해보시는 것을 추천합니다. ";
                    
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case "ClearPinNotePadQuiz":
                {
                    StartCoroutine("SendHintMessage");
                    guidesDictionary[guideType] = true;

                    break;
                }

            default:
                {
                    break;
                }
        }
    }

    private IEnumerator SendHintMessage()
    {
        foreach (string str in pinNotPadHintChatting)
        {
            EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
            yield return new WaitForSeconds(1f);

        }
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 1f);
    }

    private void OnApplicationQuit()
    {
        isZooglePinNotePadOpenCheck = false;

        //디버그용
        foreach(var guide in guideStatusesList) 
        {
            guide.isCompleted = false;
        }

        EventManager.StopListening(ECoreEvent.OpenPlayGuide, OnPlayGuide);
    }
}
