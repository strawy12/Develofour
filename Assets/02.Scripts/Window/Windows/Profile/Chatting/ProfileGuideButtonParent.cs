using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButtonParent : MonoBehaviour
{
    public Action OnClickGuideButton;

    [SerializeField]
    private ProfileGuideButton guideButtonPrefab;

    [SerializeField]
    private Transform poolParent;

    private List<ProfileGuideButton> guideButtonList;

    private Queue<ProfileGuideButton> poolQueue;

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoCategoryDataList;
    [SerializeField]
    private Button nextBtn;
    [SerializeField]
    private Button prevBtn;

    private int currentIndex = 0;

    public bool isWeightSizeUp = false;



    public void Init()
    {
        guideButtonList = new List<ProfileGuideButton>();
        poolQueue = new Queue<ProfileGuideButton>();
        CreatePool();
        infoCategoryDataList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.AddGuideButton, AddButtonOnActiveNewCategory);
        EventManager.StartListening(EProfileEvent.AddGuideButton, CheckRemoveBtn);
        prevBtn.onClick.AddListener(ClickPrev);
        nextBtn.onClick.AddListener(ClickNext);
        SaveSetting();
    }
    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 15; i++)
        {
            ProfileGuideButton button = Instantiate(guideButtonPrefab, poolParent);
            button.OnClick.AddListener(delegate { OnClickGuideButton?.Invoke(); });
            button.gameObject.SetActive(false);
            poolQueue.Enqueue(button);
        }
    }

    private ProfileGuideButton PopButton()
    {
        if (poolQueue.Count <= 0)
        {
            CreatePool();
        }

        return poolQueue.Dequeue();
    }

    private void PushButton(ProfileGuideButton button)
    {
        guideButtonList.Remove(button);
        button.Releasse();
        button.gameObject.SetActive(false);
        poolQueue.Enqueue(button);
    }
    #endregion
    #region ButtonSetting
    private void SaveSetting()
    {
        foreach (var categoryData in infoCategoryDataList)
        {
            if (!DataManager.Inst.IsCategoryShow(categoryData.Key))
            {
                continue;
            }

            foreach (var infoTextData in categoryData.Value.infoTextList)
            {
                AddButton(infoTextData);
            }
        }
    }

    private void AddButtonOnActiveNewCategory(object[] ps)
    {
        if (!(ps[0] is EProfileCategory))
        {
            return;
        }
        Debug.Log("AddButtonOnActiveCategory");

        EProfileCategory category = (EProfileCategory)ps[0];
        foreach (var infoText in infoCategoryDataList[category].infoTextList)
        {
            AddButton(infoText);
        }
    }

    public void AddButton(ProfileInfoTextDataSO data)
    {
        if (data.guideTopicName == EGuideTopicName.None || DataManager.Inst.IsProfileInfoData(data.category, data.key))
        {
            return;
        }
        ProfileGuideButton button = PopButton();
        if (button == null)
        {
            return;
        }

        button.transform.SetParent(transform);
        button.Init(data);
        guideButtonList.Add(button);

        UpdateButton();
    }

    private void CheckRemoveBtn(object[] ps)
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }
        Debug.Log("CheckRemoveBtn");

        string key = ps[1] as string;
        if (key == null)
        {
            return;
        }
        EProfileCategory category = (EProfileCategory)ps[0];
        ProfileGuideButton button = guideButtonList.Find((x) => x.InfoData.key == key);

        if (button == null)
        {
            return;
        }

        PushButton(button);
        UpdateButton();
    }
    #endregion
    #region Page
    public void UpdateButton()
    {
        if(currentIndex >= guideButtonList.Count)
        {
            currentIndex = currentIndex <= 0 ? 0 : guideButtonList.Count - 1;
        }
        
        if (isWeightSizeUp == false)
        {
            for (int i = 0; i < guideButtonList.Count; i++)
            {
                guideButtonList[i].gameObject.SetActive(false);
            }
            guideButtonList[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < guideButtonList.Count; i++)
            {
                guideButtonList[i].gameObject.SetActive(false);
            }
            guideButtonList[currentIndex].gameObject.SetActive(true);
            int tempIndex = currentIndex;
            for (int i = 0; i < 2; i++)
            {
                tempIndex++;
                if (tempIndex >= guideButtonList.Count)
                {
                    break;
                }
                guideButtonList[tempIndex].gameObject.SetActive(true);
            }
        }
    }
    private void ClickNext()
    {
        if (currentIndex + 1 >= guideButtonList.Count)
        {
            return;
        }
        else
        {
            currentIndex++;
        }

        UpdateButton();
    }
    private void ClickPrev()
    {
        if (currentIndex - 1 < 0)
        {
            return;
        }
        else
        {
            currentIndex--;
        }

        UpdateButton();
    }
    #endregion
    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.AddGuideButton, AddButtonOnActiveNewCategory);
        EventManager.StopListening(EProfileEvent.AddGuideButton, CheckRemoveBtn);
        OnClickGuideButton = null;
    }
}
