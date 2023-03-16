using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    SuspectProfileInfomation,
    SuspectProfileExtensionInfomation,
    BlogTagInfo,
    SNSTagInfo,
}
public class ProfilePanel : MonoBehaviour
{
    
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();
    [SerializeField]
    private List<ProfileInfoDataSO> infoDataList = new List<ProfileInfoDataSO>();

    public void Init()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);


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

        GetInfoPanel(category).ChangeValue(ps[1] as string);

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
