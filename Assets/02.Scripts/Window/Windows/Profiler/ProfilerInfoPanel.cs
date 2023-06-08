using System.Collections;
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


    public void Init()
    {

        infoTextQueue = new Queue<ProfilerInfoText>();
        infoTextList = new List<ProfilerInfoText>();
        //currentImage = GetComponent<Image>();
        //currentImage.material = Instantiate(currentImage.material);
        CreatePool();
        EventManager.StartListening(EProfilerEvent.ShowInfoPanel, Show);
        EventManager.StartListening(EProfilerEvent.HideInfoPanel, Hide);

        Hide();
    }


    public void ChangeValue(EProfilerCategory category, int id)
    {
        if(currentData == null)
        {
            return;
        }
        if (category != currentData.category)
        {
            return;
        }

        foreach (var infoText in infoTextList)
        {
            if (infoText.InfoData.id == id)
            {
                infoText.Show();
            }
            InfoTextListRefresh();
        }
    }

    private void InfoTextListRefresh()
    {
        infoTextList.ForEach((info) => { info.RefreshSize(); });
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
        SpriteSetting();

        if(currentData.defaultInfoText != null)
        {
            ProfilerInfoText infoText = Pop();
            infoText.Setting(currentData.defaultInfoText);
            infoText.Show();
            infoText.gameObject.SetActive(true);
        }

        foreach (var infoData in currentData.infoTextList)
        {
            ProfilerInfoText infoText = Pop();
            infoText.Setting(infoData);

            if (DataManager.Inst.IsProfilerInfoData(infoText.InfoData.id))
            {
                infoText.Show();
                infoText.gameObject.SetActive(true);
            }
        }
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

    public string SetInfoText(int id)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (id == infoText.InfoData.id)
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

    }
    //private void FillPostItColor()
    //{
    //DOTween.To(
    //    () => currentImage.material.GetFloat("_Dissolve"),
    //    (v) => currentImage.material.SetFloat("_Dissolve", v),
    //    1f, 3f);
    //}
}