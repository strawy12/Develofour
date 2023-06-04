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

    private List<ProfileGuideDataSO> guideDataList; 
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

    public void Init(List<ProfileGuideDataSO> guideDataSOs)
    {
        guideButtonList = new List<ProfileGuideButton>();
        poolQueue = new Queue<ProfileGuideButton>();
        guideDataList = guideDataSOs;
        CreatePool();
        EventManager.StartListening(EProfileEvent.AddGuideButton, AddGuideButton);
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
        if(DataManager.Inst.GetIsClearTutorial())
        {
            AddGuideButton();
        }
    }

    private void AddGuideButton(object[] ps = null)
    {
        foreach(var data in guideDataList)
        {
            AddButton(data);
        }
    }

    public void AddButton(ProfileGuideDataSO data)
    {
        ProfileGuideButton button = guideButtonList.Find(x => x.GuideData == data);
        if (button != null)
        {
            return;
        }
        button = PopButton();

        button.transform.SetParent(transform);
        button.Init(data);
        button.OnClick.AddListener(delegate { OnClickGuideButton?.Invoke(); });
        guideButtonList.Add(button);

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
        EventManager.StopListening(EProfileEvent.AddGuideButton, AddGuideButton);
    }
}