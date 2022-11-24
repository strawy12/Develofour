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

    private void Awake()
    {
        favoriteBtn = GetComponent<Button>();
    }

    public void Init()
    {
        //OnClick.AddListener(() => EventManager.TriggerEvent<EBrowserEvent>(EBrowserEvent.OnOpenSite, new object[] { siteLink, 0 }));
    }

    public void Init(Sprite iconSprite, string title)
    {
        if (iconSprite != null)
        {
            iconImage.sprite = iconSprite;

        }
        if (title != null)
        {
            titleText.text = title;
        }
    }


}