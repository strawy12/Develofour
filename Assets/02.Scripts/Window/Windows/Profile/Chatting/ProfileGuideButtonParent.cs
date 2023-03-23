using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButtonParent : MonoBehaviour
{
    [SerializeField]
    private ProfileGuideButton guideButtonPrefab;

    [SerializeField]
    private Transform poolParent;

    private List<ProfileGuideButton> guideButtonList;

    private Queue<ProfileGuideButton> poolQueue;

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoCategoryDataList;

    public void Init()
    {
        guideButtonList = new List<ProfileGuideButton>();
        CreatePool();
        infoCategoryDataList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.AddGuideButton, AddButtonOnActiveNewCategory);

        SaveSetting();
    }
    #region Pool
    private void CreatePool()
    {
        for(int i = 0; i < 15; i++)
        {
            ProfileGuideButton button = Instantiate(guideButtonPrefab, poolParent);

            button.gameObject.SetActive(false);
            poolQueue.Enqueue(button);
        }
    }

    private ProfileGuideButton PopButton()
    {
        if(poolQueue.Count <= 0)
        {
            CreatePool();
        }

        return poolQueue.Dequeue();
    }

    private void PushButton(ProfileGuideButton button)
    {
        guideButtonList.Remove(button);
        button.gameObject.SetActive(false);
        poolQueue.Enqueue(button);
    }
    #endregion

    private void SaveSetting()
    {
        foreach(var categoryData in infoCategoryDataList)
        {
            if (!DataManager.Inst.IsCategoryShow(categoryData.Key))
            {
                continue;
            }

            foreach(var infoTextData in categoryData.Value.infoTextList)
            {
                AddButton(infoTextData);
            }
        }
    }

    private void AddButtonOnActiveNewCategory(object[] ps)
    {
        if(!(ps[0] is EProfileCategory))
        {
            return;
        }
        EProfileCategory category = (EProfileCategory)ps[0];
        foreach (var infoText in ResourceManager.Inst.GetProfileCategoryData(category).infoTextList)
        {
            AddButton(infoText);
        }
    }

    public void AddButton(ProfileInfoTextDataSO data)
    {
        if(data.guideTopicName == EGuideTopicName.None || DataManager.Inst.IsGuideUse(data.guideTopicName))
        {
            return;
        }
        ProfileGuideButton button  = guideButtonList.Find(x => x.InfoData == data);
        if(button != null)
        {
            return;
        }

        button = PopButton();
        button.transform.SetParent(transform);
        button.Init(data);
        button.gameObject.SetActive(true);
    }


    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.AddGuideButton, AddButtonOnActiveNewCategory);
    }
}
