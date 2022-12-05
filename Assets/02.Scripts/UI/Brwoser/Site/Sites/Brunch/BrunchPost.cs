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

    private BrunchPostDataSO postData;
    public Action<BrunchPost> OnRemove;
    public BrunchPostDataSO PostData { get { return postData; } }
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
        this.postData = postData;
    }

    public void Remove() 
    {
        OnRemove?.Invoke(this);

        removeBtn.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenRemovePanel()
    {
        removeBtn.gameObject.SetActive(true);
    }
}
