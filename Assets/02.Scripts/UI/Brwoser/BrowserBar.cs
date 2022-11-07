using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class BrowserBar : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button undoBtn;
    [SerializeField] private Button redoBtn;

    public UnityEvent OnClose { get { return closeBtn.onClick; } }
    public UnityEvent OnRedo { get { return redoBtn.onClick; } }
    public UnityEvent OnUndo { get { return undoBtn.onClick; } }

    [SerializeField] private TMP_Text addressText;
    [SerializeField] private Image siteIcon;
    [SerializeField] private TMP_Text siteTitleText;

    [SerializeField] private Transform favoritesParent;
    [SerializeField] private FavoriteButton favoriteBtnPrefab;
    private List<FavoriteButton> favoritesList = new List<FavoriteButton>();

    public void Init()
    {
        EventManager.StartListening(EEvent.AddFavoriteSite, AddFavoritesButton);        
    }

    public void ChangeSiteData(SiteData siteData) 
    {
        //siteIcon.sprite = siteData.siteIconSprite;
        //siteTitleText.text = siteData.siteTitle;
        //addressText.text = siteData.address;
    }
    
    public void AddFavoritesButton(object param)
    {
        if (param == null || !(param is ESiteLink)) return;

        foreach(FavoriteButton favoriteButton in favoritesList)
        {
            if(favoriteButton.SiteLink == param)
            {
                return;
            } 
        }
        FavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
        button.SiteLink = param;
        //button.OnClick.AddListener() 스태틱이벤트 연결
        favoritesList.Add(button);
    }
}
