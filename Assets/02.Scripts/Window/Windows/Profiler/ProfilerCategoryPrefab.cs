using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

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
    public void Init(Action clickAction)
    {
        maxSize = categoryImage.rectTransform.sizeDelta;
        isSelected = false;
        OnClick = null;
        OnClick += clickAction;
    }
    #region Show/Hide
    public void Show(ProfilerCategoryDataSO categoryData)
    {
        currentData = categoryData;

        if (!DataManager.Inst.IsCategoryShow(categoryData.category) || categoryData.category == EProfilerCategory.InvisibleInformation)
        {
            return;   
        }

        gameObject.SetActive(true);

        titleName.SetText(categoryData.categoryName);
        Define.SetSprite(categoryImage, categoryData.categorySprite, maxSize);
    }

    public void Hide()
    {
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

}
