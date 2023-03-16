using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    SuspectProfileInfomation,
    SuspectProfileExtensionInfomation,
    VictimProfileInfomation,
    BlogTagInfo,
    SNSTagInfo,
}

public class ProfilePanel : MonoBehaviour
{
    
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();
    [SerializeField]
    private List<ProfileInfoDataSO> infoDataList = new List<ProfileInfoDataSO>();

    [SerializeField]
    private Sprite profilerSprite;

    public void Init()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
        EventManager.StartListening(ENoticeEvent.GeneratedProfileFindNotice, SendAlarm);
        for(int i = 0; i < infoPanelList.Count; i++)
        {
            infoPanelList[i].Init(infoDataList[i]);
        }

        foreach(var infoPanel in infoPanelList)
        {
            infoPanel.Init(infoDataList.Find(x => x.category == infoPanel.category));
        }
    }

    private void SaveShowCategory(EProfileCategory category)
    {
        foreach (var data in infoDataList)
        {
            if (data.category == category)
            {
                data.isShowCategory = true;
            }
        }
    }

    //이벤트 매니저 등록
    private void ChangeValue(object[] ps) // string 값으로 들고옴
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];

        
        if(ps[2] != null)
        {
            Debug.Log("asdffff");
            List<string> strList = ps[2] as List<string>;
            foreach(var temp in strList)
            {
                Debug.Log(GetInfoPanel(category).CheckIsTrue(temp));
                if (!GetInfoPanel(category).CheckIsTrue(temp))
                {
                    return;
                }
            }
        }

        ProfileInfoPanel categoryPanel = infoPanelList.Find(x=>x.category == category);

        if (!categoryPanel.gameObject.activeSelf)
        {
            categoryPanel.gameObject.SetActive(true);
            SaveShowCategory(category);
        }

        GetInfoPanel(category).ChangeValue(ps[1] as string);
    }

    private ProfileInfoPanel GetInfoPanel(EProfileCategory category)
    {
        foreach(var panel in infoPanelList)
        {
            if(panel.category == category)
            {
                return panel;   
            }
        }

        return null;
    }

    public void SendAlarm(object[] ps)
    {
        if(!(ps[0] is string) || !(ps[1] is string))
        {
            return;
        }

        string key = ps[1] as string;
        string answer;
        string temp = "nullError";
        foreach (var infoPanel in infoPanelList)
        {
            foreach(var infoText in infoPanel.infoTextList)
            {
                if(key == infoText.infoNameKey)
                {
                    answer = infoText.infoTitleText.text;
                    temp = answer.Replace(": ", "");
                }
            }
        }

        string text = ps[0] as string + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
        NoticeSystem.OnNotice.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profilerSprite, ENoticeTag.Profiler);
    }

    private void OnApplicationQuit()
    {
        foreach (var data in infoDataList)
        {
            data.Reset();
        }
    }


    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, ChangeValue);
    }
}
