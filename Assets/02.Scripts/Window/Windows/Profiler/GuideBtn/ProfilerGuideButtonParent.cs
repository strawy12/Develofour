using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerGuideButtonParent : MonoBehaviour
{
    public Action OnClickGuideButton;

    [SerializeField]
    private ProfilerGuideButton guideButtonPrefab;

    [SerializeField]
    private Transform poolParent;

    private List<ProfilerGuideButton> guideButtonList;

    private Queue<ProfilerGuideButton> poolQueue;

    private List<ProfilerGuideDataSO> guideDataList; 
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

    public void Init(List<ProfilerGuideDataSO> guideDataSOs)
    {
        Debug.Log("Asdf");
        guideButtonList = new List<ProfilerGuideButton>();
        poolQueue = new Queue<ProfilerGuideButton>();
        guideDataList = guideDataSOs;
        CreatePool();
        EventManager.StartListening(EProfilerEvent.AddGuideButton, AddGuideButton);
        prevBtn.onClick.AddListener(ClickPrev);
        nextBtn.onClick.AddListener(ClickNext);
        SaveSetting();
    }
    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 15; i++)
        {
            ProfilerGuideButton button = Instantiate(guideButtonPrefab, poolParent);
            button.gameObject.SetActive(false);
            poolQueue.Enqueue(button);
        }
    }

    private ProfilerGuideButton PopButton()
    {
        if (poolQueue.Count <= 0)
        {
            CreatePool();
        }

        return poolQueue.Dequeue();
    }

    private void PushButton(ProfilerGuideButton button)
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
        guideDataList.Clear();
        foreach(var guide in DataManager.Inst.SaveData.profilerGuideData)
        {
            if(guide.isAdd == true)
            {
                if(ResourceManager.Inst.GetProfilerGuideDataSO(guide.guideName) != null)
                {
                    guideDataList.Add(ResourceManager.Inst.GetProfilerGuideDataSO(guide.guideName));
                }
            }
        }
        if (ps == null)
        {
            foreach (var data in guideDataList)
            {
                AddButton(data);
            }
        }
        else
        {
            if (!(ps[0] is string)) return;
            string name = ps[0] as string;
            AddButton(guideDataList.Find(x => x.guideName == name));
        }
    }

    public void AddButton(ProfilerGuideDataSO data)
    {
        if (data == null)
            return;

        ProfilerGuideButton button = guideButtonList.Find(x => x.GuideData == data);
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
        EventManager.StopListening(EProfilerEvent.AddGuideButton, AddGuideButton);
    }
}