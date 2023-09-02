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

    private Vector2 defaultMinSize;

    public bool isSelected { get; private set; }

    private Action OnClick;
    public void Init(Action clickAction)
    {
        defaultMinSize = categoryImage.rectTransform.sizeDelta;
        isSelected = false;
        OnClick = null;
        OnClick += clickAction;
        EventManager.StartListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StartListening(EProfilerEvent.Minimum, SetSize);
    }
    #region Show/Hide
    public void Show(ProfilerCategoryDataSO categoryData)
    {
        currentData = categoryData;

        if (!DataManager.Inst.IsCategoryShow(categoryData.id) 
            || Constant.ProfilerCategoryKey.CheckInvisible(categoryData.id))
        {
            return;
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
        //if (!ProfilerWindow.CurrentProfiler.originWindowAlteration.isMaximum)
        //{
        //    categoryImage.rectTransform.sizeDelta = defaultMinSize;
        //}
        //else
        //{

        //}
    }

    public void Hide()
    {
        EventManager.StopListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StopListening(EProfilerEvent.Minimum, SetSize);
        gameObject.SetActive(false);
        UnSelect();
        categoryImage.rectTransform.sizeDelta = defaultMinSize;
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

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.Maximum, SetSize);
        EventManager.StopListening(EProfilerEvent.Minimum, SetSize);
    }
}
