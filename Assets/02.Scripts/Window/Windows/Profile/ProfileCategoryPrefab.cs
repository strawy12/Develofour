using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ProfileCategoryPrefab : MonoBehaviour, IPointerClickHandler
{
    private ProfileCategoryDataSO currentData;
    public ProfileCategoryDataSO CurrentData
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
    public void Show(ProfileCategoryDataSO categoryData)
    {
        currentData = categoryData;

        if (!DataManager.Inst.IsCategoryShow(categoryData.category))
        {
            return;   
        }

        gameObject.SetActive(true);

        titleName.SetText(Define.TranslateInfoCategory(categoryData.category));
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
        EventManager.TriggerEvent(EProfileEvent.ShowInfoPanel, new object[1] { currentData });
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
