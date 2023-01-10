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
    private BrunchPostRemoveButton removeBtn;

    private BrunchPostDataSO postData;
    public Action<BrunchPost> OnRemove;
    public BrunchPostDataSO PostData { get { return postData; } }
    private void Awake()
    {
        settingBtn.onClick.AddListener(OpenRemovePanel);
        removeBtn.Onclick.AddListener(Remove);
    }

    public void Init(BrunchPostDataSO postData)
    {
        novelTitleText.text = postData.wirteTitle;
        novelScriptsText.text = postData.wirteInfo;
        novelInfoText.text = postData.wirteDate;
        novelImage.sprite = postData.writeImage;
        this.postData = postData;
        removeBtn.Init();
    }

    public void Remove() 
    {
        OnRemove?.Invoke(this);

        removeBtn.Close();
        gameObject.SetActive(false);
    }

    public void OpenRemovePanel()
    {
        removeBtn.Open();
    }
}
