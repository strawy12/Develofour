using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BrowserFavoriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button favoriteBtn;

    [SerializeField]
    private ESiteLink siteLink;
    [SerializeField]
    private GameObject highlightPanel;

    public ESiteLink SiteLink
    {
        get
        {
            return siteLink;
        }
        set
        {
            siteLink = value;
        }
    }

    public UnityEvent OnClick 
    { 
        get 
        {
            favoriteBtn ??= GetComponent<Button>();
            return favoriteBtn.onClick; 
        } 
    }

    public void Init(Sprite iconSprite, string title)
    {
        favoriteBtn ??= GetComponent<Button>();

        iconImage.sprite = iconSprite;
        titleText.text = title;

        OnClick.AddListener(() => OpenTriggerWindow());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightPanel == null) return;
        highlightPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightPanel == null) return;
        highlightPanel.SetActive(false);
    }

    public void OpenTriggerWindow()
    {
        object[] ps = new object[3] { siteLink, Constant.LOADING_DELAY, true };
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, ps);
    }

}