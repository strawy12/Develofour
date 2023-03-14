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

    private Dictionary<EProfileCategory, ProfileCategoryPanel> categoryPanels = new Dictionary<EProfileCategory, ProfileCategoryPanel>();
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();
    [SerializeField]
    private List<ProfileInfoDataSO> infoDataList = new List<ProfileInfoDataSO>();
    private bool clearProfilerTutorial;

    public void Init()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
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
    private void ChangeValue(object[] ps) // 0 = 카테고리, 1 = key값 스트링, 
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];

        Debug.Log("Get Category : " + categoryPanels[category]);
        if (categoryPanels.ContainsKey(category))
        {

            Debug.Log("ShowCategory");
            ProfileCategoryPanel categoryPanel = categoryPanels[category];

            if (!categoryPanel.gameObject.activeSelf)
            {
                Debug.Log("ShowCategory2");
                categoryPanel.gameObject.SetActive(true);
                SaveShowCategory(category);
            }
            GetInfoPanel(category).ChangeValue(ps[1] as string);
        }
        else
        {
            Debug.LogWarning("해당 CategoryKey가 존재하지않습니다.");
        }
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
