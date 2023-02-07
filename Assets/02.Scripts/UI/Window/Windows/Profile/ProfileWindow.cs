using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileWindow : Window
{
    [SerializeField]
    private ProfilePanel profilePanel;

    [SerializeField]
    private Button profileSystemBtn;
    [SerializeField]
    private ProfileChatting profileChatting;
    [SerializeField]
    private Button infoSearchBtn;

    protected override void Init()
    {
        base.Init();
        profileChatting.Init();
        profilePanel.Init();
        profileSystemBtn.onClick.AddListener(ShowProfileCategoryPanel);
    }
    private void ShowProfileCategoryPanel()
    {
        profilePanel.gameObject.SetActive(true);
    }
}
