using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class ProfilerCategoryPrefab : MonoBehaviour, IPointerClickHandler
{
    private ProfilerCategoryDataSO currentData;
    public ProfilerCategoryDataSO CurrentData
    {
        get
        {
            return currentData;
        }
    }
    [SerializeField]
    private TMP_Text titleName;

    [SerializeField]
    private Image categoryImage;
    [SerializeField]
    private Image selectImage;
    private Vector2 maxSize;
    public bool isSelected { get; private set; }

    private Action OnClick;

    private GuideObject guideObj;
    public GuideObject GuideObj => guideObj;

    private Vector2 defaultMinSize;
    public void Init(Action clickAction)
    {
        defaultMinSize = ((RectTransform)transform).sizeDelta;
        maxSize = categoryImage.rectTransform.sizeDelta;
        isSelected = false;
        OnClick = null;
        OnClick += clickAction;
        if(guideObj == null)
        {
            guideObj = GetComponent<GuideObject>();
            if (guideObj == null) Debug.LogError("가이드 오브젝트가 NULL입니다");
        }

        if (guideObj != null)
            guideObj.Init();

        EventManager.StartListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StartListening(EProfilerEvent.Minimum, SetSize);
    }
    #region Show/Hide
    public void Show(ProfilerCategoryDataSO categoryData)
    {
        currentData = categoryData;

        if (!DataManager.Inst.IsCategoryShow(categoryData.ID) || categoryData.categoryType == EProfilerCategoryType.Visiable)
        {
            return;   
        }

        Debug.Log("프로파일 카테고리 프리팹 쇼 " + categoryData.categoryType);

        if(DataManager.Inst.IsPlayingProfilerTutorial())
        {
            if(categoryData.categoryType == EProfilerCategoryType.Character)
            {
                Debug.Log("프로파일 카테고리 프리팹 character bool false");
                ProfilerTutorial.IsExistCharacterTODO = false;

            }
            if (categoryData.categoryType == EProfilerCategoryType.Info) 
            {
                Debug.Log("프로파일 카테고리 프리팹 incidnet bool false");
                ProfilerTutorial.IsExistIncidentTODO = false;
            }
        }

        gameObject.SetActive(true);

        titleName.SetText(categoryData.categoryName);

        EventManager.StartListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StartListening(EProfilerEvent.Minimum, SetSize);

        categoryImage.sprite = currentData.categorySprite;
        SetSize();
    }

    private void SetSize(object[] ps = null)
    {
        float x1, y1, x2, y2;
        float maxSize = (categoryImage.transform.parent as RectTransform).rect.width - 5;

        if (currentData.categorySprite == null)
        {
            return;
        }
        
        if (!ProfilerWindow.currentProfiler.isMaximum)
        {
            categoryImage.rectTransform.sizeDelta = defaultMinSize;
            return;
        }
        else
        {
            if (currentData.categorySprite.rect.width != currentData.categorySprite.rect.height)
            {
                x1 = currentData.categorySprite.rect.width;
                y1 = currentData.categorySprite.rect.height;
                if (x1 > y1)
                {
                    x2 = maxSize;
                    y2 = y1 * x2 / x1;
                }
                else
                {
                    y2 = maxSize;
                    x2 = x1 * y2 / y1;
                }
            }
            else
            {
                x2 = y2 = maxSize;
            }

            categoryImage.rectTransform.sizeDelta = new Vector2(x2, y2);
        }
    }
    public void Hide()
    {
        EventManager.StopListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StopListening(EProfilerEvent.Minimum, SetSize);
        gameObject.SetActive(false);
        UnSelect();
        categoryImage.rectTransform.sizeDelta = maxSize;
    }
    #endregion 
    #region EventSystem

    public void OnPointerClick(PointerEventData eventData)
    {
        Select();
    }
    #endregion

    public void Select()
    {
        if (isSelected)
        {
            return;
        }
        OnClick?.Invoke();
        TutorialClickCheck();
        selectImage.gameObject.SetActive(true);
        isSelected = true;
        EventManager.TriggerEvent(EProfilerEvent.ShowInfoPanel, new object[1] { currentData });
    }

    public void UnSelect()
    {
        if (!isSelected)
        {
            return;
        }
        isSelected = false;
        selectImage.gameObject.SetActive(false);

    }

    public void TutorialClickCheck()
    {
        if(DataManager.Inst.IsPlayingProfilerTutorial())
        {
            if (currentData.categoryType == EProfilerCategoryType.Character)
            {
                EventManager.TriggerEvent(ETutorialEvent.ClickCharacterCategory);
            }
            else if (currentData.categoryType == EProfilerCategoryType.Info)
            {
                EventManager.TriggerEvent(ETutorialEvent.ClickIncidentCategory);
            }
        }
    }

}
