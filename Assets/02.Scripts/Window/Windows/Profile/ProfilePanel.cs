using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EProfileCategory
{
    Owner,
    Boyfriend,
}

public enum OwnerCategory
{
    Email,
    Name
}
public enum BoyfriendCategory
{
    Age,
    Birth
}

public class ProfilePanel : MonoBehaviour
{

    [SerializeField]
    private GameObject CategoryPanel;
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();
    [SerializeField]
    private List<ProfileInfoDataSO> infoDataList = new List<ProfileInfoDataSO>();
    private bool clearProfilerTutorial;

    public void Init()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
        for(int i = 0; i < infoPanelList.Count; i++)
        {
            infoPanelList[i].Init(infoDataList[i]);
        }
    }

    private void CategoryStartEvent(object[] ps)
    {
        ProfileCategoryPanel panel = ps[0] as ProfileCategoryPanel;
        panel.StopCor();
    }

    private void CategoryEndEvent(object[] ps)
    {
        EventManager.StopListening(ETutorialEvent.ProfileInfoEnd, CategoryStartEvent);
        EventManager.StopListening(ETutorialEvent.ProfileEventStop, CategoryEndEvent);
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

        GetInfoPanel(category).ChangeValue(ps[1] as string);

        //현재는 카테고리 패널을 사용하지 않음
        // ProfileCategoryPanel categoryPanel = categoryPanels[category];
        //Debug.Log("Get Category : " + categoryPanels[category]);
        //if (categoryPanels.ContainsKey(category))
        //{

        //    Debug.Log("ShowCategory");
        //    ProfileCategoryPanel categoryPanel = categoryPanels[category];

        //    if (!categoryPanel.gameObject.activeSelf)
        //    {
        //        Debug.Log("ShowCategory2");
        //        categoryPanel.gameObject.SetActive(true);
        //        SaveShowCategory(category);
        //    }
        //    GetInfoPanel(category).ChangeValue(ps[1] as string);
        //}
        //else
        //{
        //    Debug.LogWarning("해당 CategoryKey가 존재하지않습니다.");
        //}

        //main에서
        //if (data != null)
        //{
        //    Debug.Log("ShowCategory");
        //    ProfileInfoPanel categoryPanel = categoryPanels[category];

        //    if (!categoryPanel.gameObject.activeSelf)
        //    {
        //        Debug.Log("ShowCategory2");
        //        categoryPanel.gameObject.SetActive(true);
        //        SaveShowCategory(category);
        //    }
        //    GetInfoPanel(category).ChangeValue(ps[1] as string);
        //}
        //else
        //{
        //    Debug.LogWarning("해당 CategoryKey가 존재하지않습니다.");
        //}

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

    public void HideCategoryParentPanel()
    {
        CategoryPanel.gameObject.SetActive(false);
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
