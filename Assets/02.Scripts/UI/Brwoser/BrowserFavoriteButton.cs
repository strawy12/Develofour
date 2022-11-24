using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BrowserFavoriteButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text titleText;

    private Button favoriteBtn;
    [SerializeField]
    private ESiteLink siteLink;

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
    public UnityEvent OnClick { get { return favoriteBtn.onClick; } }


    public void Init(Sprite iconSprite, string title)
    {
        favoriteBtn ??= GetComponent<Button>();

        iconImage.sprite = iconSprite;
        titleText.text = title;

        OnClick.AddListener(() => OpenTriggerWindow());

    }

    public void OpenTriggerWindow()
    {
        object[] ps = new object[2] { siteLink, Constant.LOADING_DELAY };
        Debug.Log(siteLink);
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, ps);
    }

}