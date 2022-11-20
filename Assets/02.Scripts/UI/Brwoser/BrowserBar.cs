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
    private FavoriteBar favoriteBar;
    public void Init()
    {
        favoriteBar = GetComponent<FavoriteBar>();
        favoriteBar.Init();
    }

    public void ChangeSiteData(SiteData siteData) 
    {
        siteIcon.sprite = siteData.siteIconSprite;
        siteTitleText.text = siteData.siteTitle;
        addressText.text = siteData.address;
    }
    
  
}
