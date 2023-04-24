using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileInfoPanel : MonoBehaviour
{
    private ProfileCategoryDataSO currentData;
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private ProfileInfoText infoTextPrefab;
    [SerializeField]
    private Transform infoTextParent;
    private Queue<ProfileInfoText> infoTextQueue;

    private List<ProfileInfoText> infoTextList;

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfileInfoText infoText = Instantiate(infoTextPrefab, infoTextParent);
            infoText.Init();
            infoText.Hide();
            infoTextQueue.Enqueue(infoText);
        }
    }

    private ProfileInfoText Pop()
    {
        if (infoTextQueue.Count == 0)
        {
            CreatePool();
        }

        ProfileInfoText infoText = infoTextQueue.Dequeue();
        infoTextList.Add(infoText);
        return infoText;
    }

    public void Push(ProfileInfoText infoText)
    {
        if (infoTextList.Contains(infoText))
        {
            infoText.Hide();
            infoTextList.Remove(infoText);
            infoTextQueue.Enqueue(infoText);
        }
    }
    public void PushAll()
    {
        foreach(var infoText in infoTextList)
        {
            infoTextQueue.Enqueue(infoText);
            infoText.Hide();
        }
        infoTextList.Clear();
    }
    #endregion

    private Image currentImage;


    public void Init()
    {
        infoTextQueue = new Queue<ProfileInfoText>();
        infoTextList = new List<ProfileInfoText>();
        currentImage = GetComponent<Image>();
        currentImage.material = Instantiate(currentImage.material);
        CreatePool();
    }


    public void ChangeValue(string key)
    {
        foreach (var infoText in infoTextList)
        {
            if (infoText.textDataSO.key == key)
            {
                if (gameObject.activeSelf == false)
                {
                    SendNotice();
                }

                DataManager.Inst.AddProfileinfoData(currentData.category, key);
                if (currentData.category != EProfileCategory.InvisibleInformation)
                {
                    EventManager.TriggerEvent(EProfileEvent.RemoveGuideButton, new object[2] { currentData.category, key });
                }

                if (key == "SuspectName" && DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler))
                {
                    EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
                }
            }
        }

        if (GetIsFindAll())
        {
            FillPostItColor();
        }
    }

    private void SendNotice()
    {
        //string head, string body, float delay, bool canDelete, Sprite icon, Color color, ENoticeTag noticeTag

        string head = "새로운 카테고리가 추가되었습니다";
        string body = "";
        if (currentData.category != EProfileCategory.InvisibleInformation)
        {
            body = $"새 카테고리 {Define.TranslateInfoCategory(currentData.category)}가 추가되었습니다.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);

    }

    public void Show(ProfileCategoryDataSO categoryData)
    {
        currentData = categoryData;
        foreach (var infoList in currentData.infoTextList)
        {
            ProfileInfoText infoText = Pop();
        }

        titleText.SetText(Define.TranslateInfoCategory(currentData.category));

        foreach (var infoData in currentData.infoTextList)
        {
            if (DataManager.Inst.IsProfileInfoData(currentData.category, infoData.key) == false)
            {
                continue;
            }
            foreach (var infoText in infoTextList)
            {
                if (infoText.textDataSO.key == infoData.key)
                {
                    infoText.Show();
                }
            }
        }

        if (GetIsFindAll())
        {
            FillPostItColor();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        PushAll();
    }


    public bool GetIsFindAll()
    {
        foreach (var info in infoTextList)
        {
            if (info.IsFind == false)
            {
                return false;
            }
        }
        return true;
    }

    public string SetInfoText(string key)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (key == infoText.textDataSO.key)
            {
                answer = infoText.textDataSO.noticeText;
            }
        }

        return answer;
    }

    private void FillPostItColor()
    {
        DOTween.To(
            () => currentImage.material.GetFloat("_Dissolve"),
            (v) => currentImage.material.SetFloat("_Dissolve", v),
            1f, 3f);
    }
}