using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class FavoriteButton : MonoBehaviour
{
    [SerializeField]private Image iconImage;
    [SerializeField]private TMP_Text titleText;

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
        Init();
    }

    public void Init()
    {
        OnClick.AddListener(() => Browser.OnOpenSite.Invoke(siteLink));
    }

    public void Init(Sprite iconSprite, string title)
    {
        iconImage.sprite = iconSprite;
        titleText.text = title;
    }


}
