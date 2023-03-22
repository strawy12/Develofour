using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    None,
    SuspectProfileInfomation,
    SuspectProfileExtensionInfomation,
    VictimProfileInfomation,
    PetInformation,
    Count,
}

public class ProfilePanel : MonoBehaviour
{
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList;

    [SerializeField]
    private Sprite profilerSpeite;

    public void Init()
    {
        infoList = ResourceManager.Inst.GetProfileCategoryDataList();

        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);


        foreach(var info in infoList)
        {
            infoPanelList.Find(x => x.category == info.Key).Init(info.Value);
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
            List<string> strList = ps[2] as List<string>;
            foreach(var temp in strList)
            {
                if (!DataManager.Inst.IsProfileInfoData(category, temp))
                {
                    return;
                }
            }
        }

        ProfileInfoPanel categoryPanel = infoPanelList.Find(x=>x.category == category);

        if (!categoryPanel.gameObject.activeSelf)
        {
            categoryPanel.gameObject.SetActive(true);

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
        NoticeSystem.OnNotice?.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profilerSpeite, Color.white, ENoticeTag.Profiler);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, ChangeValue);
    }
}
