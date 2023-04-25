using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfileCategoryPrefab : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private ProfileCategoryDataSO currentData;

    [SerializeField]
    private TMP_Text titleName;

    [SerializeField]
    private Image categoryImage;

    [SerializeField]
    private GameObject highlightImage;

    private Vector2 maxSize;

    public void Init()
    {
        maxSize = categoryImage.rectTransform.sizeDelta;
    }
    #region Show/Hide
    public void Show(ProfileCategoryDataSO categoryData)
    {
        currentData = categoryData;
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EProfileEvent.ShowInfoPanel, new object[1] { currentData });
    }
    #endregion

    private void SendNotice()
    {
        string head = "새로운 카테고리가 추가되었습니다";
        string body = "";
        if (currentData.category != EProfileCategory.InvisibleInformation)
        {
            body = $"새 카테고리 {Define.TranslateInfoCategory(currentData.category)}가 추가되었습니다.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);
    }

}
