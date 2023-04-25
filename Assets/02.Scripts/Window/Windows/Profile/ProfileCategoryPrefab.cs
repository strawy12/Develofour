using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private Vector2 maxSize;

    public void Init()
    {
        maxSize = categoryImage.rectTransform.sizeDelta;
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
        categoryImage.rectTransform.sizeDelta = maxSize;
    }
    #endregion 
    #region EventSystem

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EProfileEvent.ShowInfoPanel, new object[1] { currentData });
    }
    #endregion

    private void SendNotice()
    {
        string head = "���ο� ī�װ��� �߰��Ǿ����ϴ�";
        string body = "";
        if (currentData.category != EProfileCategory.InvisibleInformation)
        {
            body = $"�� ī�װ� {Define.TranslateInfoCategory(currentData.category)}�� �߰��Ǿ����ϴ�.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);
    }

}
