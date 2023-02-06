using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ProfileCategoryPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    public EProfileCategory profileCategory;

    private ProfileInfoPanel infoPanel;

    [SerializeField]
    private Button InfoBtn;
    //info패널을 들고있음
    public void Init(EProfileCategory categoryEnum, string name, ProfileInfoPanel infoPanel)
    {
        profileCategory = categoryEnum;
        nameText.text = name;
        this.infoPanel = infoPanel;
        InfoBtn.onClick.AddListener(ShowInfoPanel);
    }

    public void ChangeValue(string key)
    {
        infoPanel.Setting(key);
    }
    private void ShowInfoPanel()
    {
        infoPanel.gameObject.SetActive(true);
    }
}
