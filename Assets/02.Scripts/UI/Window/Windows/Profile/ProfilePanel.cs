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
    //?? 어떻게든 저장가능 , 동적으로 SO생성시키고 
    [SerializeField]
    private ProfileCategoryPanel categoryPanelPrefab;
    [SerializeField]
    private Transform categoryPanelParent;
    private Dictionary<EProfileCategory, ProfileCategoryPanel> categoryPanels = new Dictionary<EProfileCategory, ProfileCategoryPanel>();
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();
    [SerializeField]
    private List<ProfileInfoDataSO> infoDataList = new List<ProfileInfoDataSO>();

    public void Init()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
        CreateCategoryPanel();
    }

    private ProfileInfoPanel GetInfoPanel(EProfileCategory category)
    {
        foreach (ProfileInfoPanel panel in infoPanelList)
        {
            if(panel.category == category)
            {
                return panel;
            }
        }
        Debug.Log("동일한 category의 InfoPanel이 존재하지 않습니다.");
        return null;
    }

    private void CreateCategoryPanel()
    {
        foreach(ProfileInfoDataSO data in infoDataList)
        {
            ProfileCategoryData categoryData = data.categoryData;
            ProfileCategoryPanel categoryPanel = Instantiate(categoryPanelPrefab, categoryPanelParent);
            ProfileInfoPanel infoPanel = GetInfoPanel(data.category);
            
            categoryPanel.Init(data.category, categoryData.categoryNameText, infoPanel);
            infoPanel.Init(data);

            if(data.isShowCategory)
            {
                categoryPanel.gameObject.SetActive(true);
            }
            else
            {
                categoryPanel.gameObject.SetActive(false);
            }

            categoryPanels.Add(data.category,categoryPanel);
        }
    }

    private void SaveShowCategory(EProfileCategory category) 
    {
        foreach(var data in infoDataList)
        {
            if(data.category == category)
            {
                data.isShowCategory = true;
            }
        }
    }

    //이벤트 매니저 등록
    private void ChangeValue(object[] ps) // 0 = 카테고리, 1 = key값 스트링, 
    {
        if(!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];
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
            categoryPanel.ChangeValue(ps[1] as string);
        }
    }
    public void HideCategoryParentPanel()
    {
        categoryPanelParent.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        foreach(var data in infoDataList)
        {
            data.Reset();
        }
    }
#endif
}
