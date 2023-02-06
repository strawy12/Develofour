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
    private Button infoSearchBtn;

    protected override void Init()
    {
        base.Init();
        Debug.Log("ProfileWindowInit");
        profilePanel.Init();
        profileSystemBtn.onClick.AddListener(ShowProfileCategoryPanel);
    }
    private void ShowProfileCategoryPanel()
    {
        profilePanel.gameObject.SetActive(true);
    }
}
