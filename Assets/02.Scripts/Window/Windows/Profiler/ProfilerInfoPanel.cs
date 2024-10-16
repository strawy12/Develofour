﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ProfilerInfoPanel : MonoBehaviour
{
    private ProfilerCategoryDataSO currentData;
    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private ProfilerTitle title;
    [SerializeField]
    private Image categoryImage;
    [SerializeField]
    private RectTransform categoryImageParent;
    [SerializeField]
    private ProfilerInfoText infoTextPrefab;
    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private Transform infoTextParent;
    private Queue<ProfilerInfoText> infoTextQueue;

    [SerializeField]
    private bool isEvidenceObj;

    public List<ProfilerInfoText> InfoTextList => infoTextList;
    private List<ProfilerInfoText> infoTextList;

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < 20; i++)
        {
            ProfilerInfoText infoText = Instantiate(infoTextPrefab, poolParent);
            infoText.Init();
            infoText.Hide();
            infoTextQueue.Enqueue(infoText);
        }
    }

    private ProfilerInfoText Pop()
    {
        if (infoTextQueue.Count == 0)
        {
            CreatePool();
        }

        ProfilerInfoText infoText = infoTextQueue.Dequeue();
        infoText.transform.SetParent(infoTextParent);
        infoTextList.Add(infoText);
        return infoText;
    }

    public void Push(ProfilerInfoText infoText)
    {
        if (infoTextList.Contains(infoText))
        {
            infoText.Hide();
            infoText.transform.SetParent(poolParent);
            infoTextList.Remove(infoText);
            infoTextQueue.Enqueue(infoText);
        }
    }
    public void PushAll()
    {
        foreach (var infoText in infoTextList)
        {
            infoTextQueue.Enqueue(infoText);
            infoText.transform.SetParent(poolParent);
            infoText.Hide();
        }
        infoTextList.Clear();
    }
    #endregion

    //private Image currentImage;


    public void Init(bool isEvidencePanel)
    {
        infoTextQueue = new Queue<ProfilerInfoText>();
        infoTextList = new List<ProfilerInfoText>();
        //currentImage = GetComponent<Image>();
        //currentImage.material = Instantiate(currentImage.material);
        CreatePool();

        EventManager.StartListening(EProfilerEvent.ShowInfoPanel, Show);
        EventManager.StartListening(EProfilerEvent.HideInfoPanel, Hide);

        if (isEvidencePanel)
        {
            EventManager.StartListening(EEvidencePanelEvent.ClickToggle, TurnOffAllToogle);

            ProfilerCategoryDataSO incidentCategory =
                ResourceManager.Inst.GetResource<ProfilerCategoryDataSO>(Constant.ProfilerCategoryKey.INCIDENT);
            if(incidentCategory == null)
            {
                Debug.LogError("사건보고서 카테고리가 없음");
                return;
            }
            
        }

        Hide();
    }

    private void TurnOffAllToogle(object[] ps)
    {
        if (!(ps[0] is ProfilerInfoText)) return;
        foreach(var infoText in infoTextList)
        {
            //일단 모든 토글 끄고
            
            //이벤트에서 보내준 자신은 켜줌
            if(infoText != ps[0] is ProfilerInfoText)
            {
                infoText.isChecked = false; 
            }
            infoText.SetImage();
        }

    }

    public void ChangeValue(string category, string infoID)
    {
        if(currentData == null)
        {
            return;
        }
        if (category != currentData.ID)
        {
            return;
        }

        foreach (var infoText in infoTextList)
        {
            if (infoText.InfoData.ID == infoID)
            {
                infoText.Show();
            }
            StartCoroutine(RefreshSizeCoroutine());
        }
    }


    public void Show(object[] ps)
    {
        if (!(ps[0] is ProfilerCategoryDataSO))
        {
            return;
        }
        EventManager.TriggerEvent(EProfilerEvent.HideInfoPanel);
        EventManager.StartListening(EProfilerEvent.Maximum, RefreshSize);
        EventManager.StartListening(EProfilerEvent.Minimum, RefreshSize);

        this.gameObject.SetActive(true);
        ProfilerCategoryDataSO categoryData = ps[0] as ProfilerCategoryDataSO;
        currentData = categoryData;
        titleText.SetText(currentData.categoryName);
        title.Setting(currentData.categoryName);
        SpriteSetting();

        bool isEvidence = false;
        if (ps.Length == 2)
        {
            isEvidence = (bool)ps[1];
        }

        if(isEvidence == true && isEvidenceObj == false)
        {
            return;
        }

        if (!string.IsNullOrEmpty(currentData.defaultInfoID))
        {
            ProfilerInfoDataSO infoData = ResourceManager.Inst.GetResource<ProfilerInfoDataSO>(currentData.defaultInfoID);
            if(infoData == null)
            {
                Debug.Log($"{currentData.defaultInfoID} == null");
            }
            ProfilerInfoText infoText = Pop();
            infoText.Setting(infoData, isEvidence);
            infoText.Show();
            infoText.gameObject.SetActive(true);
        }

        foreach (string infoID in currentData.infoIDList)
        {
            ProfilerInfoDataSO infoData = ResourceManager.Inst.GetResource<ProfilerInfoDataSO>(infoID.Trim());
            if(infoData == null)
            {
                Debug.Log($"{infoID.Trim()} null Data");
                continue;
            }
           
            ProfilerInfoText infoText = Pop();
            infoText.Setting(infoData, isEvidence);
            if (DataManager.Inst.IsProfilerInfoData(infoText.InfoData.ID))
            {
                infoText.Show();
                infoText.gameObject.SetActive(true);
            }
        }

        if(this.gameObject.activeSelf)
            StartCoroutine(RefreshSizeCoroutine());

    }
    private void RefreshSize(object[] ps)
    {
        StartCoroutine(RefreshSizeCoroutine());
    }
    private IEnumerator RefreshSizeCoroutine()
    {
        foreach (var infoText in infoTextList)
        {
            infoText.RefreshSize();
        }
        yield return new WaitForSeconds(0.02f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)infoTextParent);
    }
    public void SpriteSetting()
    {
        float x1, y1, x2, y2;
        float iconDefaultSize = categoryImageParent.rect.height - 5;
        if (currentData.categorySprite == null)
        {
            categoryImage.sprite = null;
            return;
        }
        if (currentData.categorySprite.rect.width != currentData.categorySprite.rect.height)
        {
            x1 = currentData.categorySprite.rect.width;
            y1 = currentData.categorySprite.rect.height;
            if (x1 > y1)
            {
                x2 = iconDefaultSize;
                y2 = y1 * x2 / x1;
            }
            else
            {
                y2 = iconDefaultSize;
                x2 = x1 * y2 / y1;
            }
        }
        else
        {
            x2 = y2 = iconDefaultSize;
        }

        categoryImage.sprite = currentData.categorySprite;
        categoryImage.rectTransform.sizeDelta = new Vector2(x2, y2);

    }
    public void Hide(object[] ps = null)
    {
        gameObject.SetActive(false);
        PushAll();
        EventManager.StopListening(EProfilerEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfilerEvent.Minimum, RefreshSize);
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

    public string SetInfoText(string id)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (id == infoText.InfoData.ID)
            {
                answer = infoText.InfoData.noticeText;
            }
        }

        return answer;
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.ShowInfoPanel, Show);
        EventManager.StopListening(EProfilerEvent.HideInfoPanel, Hide);
        EventManager.StopListening(EProfilerEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfilerEvent.Minimum, RefreshSize);
        EventManager.StopListening(EEvidencePanelEvent.ClickToggle, TurnOffAllToogle);
    }
 }