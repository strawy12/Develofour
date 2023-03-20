using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGuideTopicName
{
    None,
    GuestLoginGuide,
    LibraryOpenGuide,
    ClickPinNotePadHint,
    ClearPinNotePadQuiz,
    Count
}


public class GuideManager: MonoBehaviour
{
    public static Action<EGuideTopicName, float> OnPlayGuide;

    [SerializeField]
    private GuideDataListSO guideListData;

    private Dictionary<EGuideTopicName, GuideData> guideTopicDictionary;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        guideTopicDictionary = new Dictionary<EGuideTopicName, GuideData>();
        
        foreach (var guideData in guideListData.guideDataList)
        {
            guideTopicDictionary.Add(guideData.topicName, guideData);
        }

        OnPlayGuide += StartPlayGuide;

        EventManager.StartListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StartListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }


    private void StartPlayGuide(EGuideTopicName guideTopicName, float timer)
    {

        if (DataManager.Inst.IsGuideUse(guideTopicName))
        {
            return;
        }

        StartCoroutine(SetTimer(timer, guideTopicName));
    }

    private IEnumerator SetTimer(float timer, EGuideTopicName guideTopicName)
    {
        yield return new WaitForSeconds(timer);

        if (DataManager.Inst.IsGuideUse(guideTopicName))
        {
            yield break;
        }
        
        StartGudie(guideTopicName);
    }

    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch (guideTopic)
        {
            case EGuideTopicName.GuestLoginGuide:
                {
                    Debug.Log("Guide In");
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuestLoginGuideLog, 0.5f, 2);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.LibraryOpenGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuideLog1, 0.2f, 1);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });
                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz:
                {
                    StartCoroutine(SendAiMessageTexts(guideTopicDictionary[guideTopic].guideTexts));
                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private IEnumerator SendAiMessageTexts(string[] values)
    {
        foreach (string str in values)
        {
            EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
            yield return new WaitForSeconds(1f);
        }
    }

    private void ThisClearGuideTopic(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        EGuideTopicName eGuideTopic = (EGuideTopicName)ps[0];

        DataManager.Inst.SetGuide(eGuideTopic, true);
    }

    private void GuideConditionCheckClear(object[] ps)
    {
        FileSO file = (FileSO)ps[0];
        bool isZooglePinHintNoteOpen = DataManager.Inst.SaveData.isZooglePinHintNoteOpen;

        string fileLocation = file.GetFileLocation();
       

        if (fileLocation == "User\\C\\내 문서\\Zoogle\\Zoogle비밀번호\\")
        {
            if (!isZooglePinHintNoteOpen)
            {
                DataManager.Inst.SaveData.isZooglePinHintNoteOpen = true;

                OnPlayGuide(EGuideTopicName.ClickPinNotePadHint, 40);
            }
            else if (isZooglePinHintNoteOpen)
            {
                DataManager.Inst.SetGuide(EGuideTopicName.ClickPinNotePadHint, true);
            }
        }
        if (fileLocation == "User\\C\\내 문서\\Zoogle\\ZooglePIN번호\\" && isZooglePinHintNoteOpen)
        {
            DataManager.Inst.SetGuide(EGuideTopicName.ClearPinNotePadQuiz, true);
            OnPlayGuide(EGuideTopicName.ClearPinNotePadQuiz, 1200);
        }
    }
    private void OnApplicationQuit()
    {
        EventManager.StopListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StopListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }
}
