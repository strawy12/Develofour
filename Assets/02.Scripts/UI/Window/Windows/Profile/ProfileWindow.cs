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
    private InfoCheckPanel infoCheckPanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;
    [SerializeField]
    private Button infoCheckBtn;
    [SerializeField]
    private Button fileSearchBtn;
    protected override void Init()
    {
        base.Init();
        profileChatting.Init();
        profilePanel.Init();
        profileSystemBtn.onClick.AddListener(ShowProfileCategoryPanel);
        infoCheckBtn.onClick.AddListener(ShowInfoCheckPanel);
        fileSearchBtn.onClick.AddListener(ShowFileSearchPanel);
    }
    private void HideAllPanel()
    {
        profilePanel.gameObject.SetActive(false);
        infoCheckPanel.gameObject.SetActive(false);
        fileSearchPanel.gameObject.SetActive(false);

    }
    private void ShowProfileCategoryPanel()
    {
        HideAllPanel();
        profilePanel.gameObject.SetActive(true);
    }
    private void ShowInfoCheckPanel()
    {
        HideAllPanel();
        infoCheckPanel.gameObject.SetActive(true);
    }
    private void ShowFileSearchPanel()
    {
        HideAllPanel();
        fileSearchPanel.gameObject.SetActive(true);
    }
}
