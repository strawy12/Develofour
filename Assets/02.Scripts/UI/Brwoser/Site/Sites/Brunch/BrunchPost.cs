using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BrunchPost : MonoBehaviour
{
    public TextMeshProUGUI novelTitleText;
    public TextMeshProUGUI novelScriptsText;
    public TextMeshProUGUI novelInfoText;
    public Image novelImage;
    [SerializeField]
    private Button settingBtn;
    [SerializeField]
    private Button removeBtn; 
    private void Awake()
    {
        settingBtn.onClick.AddListener(OpenRemovePanel);
        removeBtn.onClick.AddListener(Remove);
    }

    public void Init(BrunchPostDataSO postData)
    {
        novelTitleText.text = postData.wirteTitle;
        novelScriptsText.text = postData.wirteInfo;
        novelInfoText.text = postData.wirteDate;
        novelImage.sprite = postData.writeImage;
    }

    public void Remove() 
    {
        removeBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenRemovePanel()
    {
        removeBtn.gameObject.SetActive(true);
    }
}
