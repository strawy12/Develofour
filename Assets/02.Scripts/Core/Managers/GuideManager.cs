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
    public Dictionary<string, bool> guidesDictionary;
    
    [SerializeField]
    private List<GuideStatusData> guideStatusesList;

    private string guideType;

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
            default:
                {
                    break;
                }
        }
    }

    private void OnApplicationQuit()
    {
        //디버그용
        foreach(var guide in guideStatusesList) 
        {
            guide.isCompleted = false;
        }

        EventManager.StopListening(ECoreEvent.OpenPlayGuide, OnPlayGuide);
    }
}
