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

    public void SetActiveBtn(bool value)
    {
        nextBtn.gameObject.SetActive(value);
        prevBtn.gameObject.SetActive(value);

    }

    public void Init()
    {
        guideButtonList = new List<ProfileGuideButton>();
        poolQueue = new Queue<ProfileGuideButton>();
        CreatePool();
        infoCategoryDataList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.AddGuideButton, AddButtonOnActiveNewCategory);
        EventManager.StartListening(EProfileEvent.RemoveGuideButton, CheckRemoveBtn);
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

        EProfileCategory category = (EProfileCategory)ps[0];
        foreach (var infoText in infoCategoryDataList[category].infoTextList)
        {
            AddButton(infoText);
        }
    }

    public void AddButton(ProfileInfoTextDataSO data)
    {
        ProfileGuideButton guideButton = guideButtonList.Find(x => x.InfoData == data);
        if (guideButton != null)
        {
            return;
        }
        if (data.guideTopicName == EGuideTopicName.None || DataManager.Inst.IsProfileInfoData(data.id) 
            || data.infoName == "" || data.infoName == "?")
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
        button.OnClick.AddListener(delegate { OnClickGuideButton?.Invoke(); });
        guideButtonList.Add(button);

        UpdateButton();
    }

    private void CheckRemoveBtn(object[] ps)
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        int id = (int)ps[1];

        EProfileCategory category = (EProfileCategory)ps[0];
        ProfileGuideButton button = guideButtonList.Find((x) => x.InfoData.id == id);

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
        if (guideButtonList.Count == 0)
        {
            return;
        }

        if (currentIndex >= guideButtonList.Count)
        {
            currentIndex = currentIndex <= 0 ? 0 : guideButtonList.Count - 1;
        }

        if (isWeightSizeUp == false)
        {
            for (int i = 0; i < guideButtonList.Count; i++)
            {
                guideButtonList[i].gameObject.SetActive(false);
            }
            if(guideButtonList.Count != 0)
            {
                guideButtonList[currentIndex].gameObject.SetActive(true);
            }
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
        if (isWeightSizeUp)
        {
            currentIndex += 3;
            if(currentIndex >= guideButtonList.Count)
            {
                int value = (currentIndex) % 3;
                currentIndex =currentIndex - (3 -  value);
            }
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
        if (isWeightSizeUp)
        {
            currentIndex -= 3;
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }
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
        EventManager.StopListening(EProfileEvent.RemoveGuideButton, CheckRemoveBtn);
    }
}
