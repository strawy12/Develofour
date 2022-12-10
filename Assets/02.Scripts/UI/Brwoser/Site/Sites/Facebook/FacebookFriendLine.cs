using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FacebookFriendLine : MonoBehaviour
{
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private GameObject selectPanel;
    [SerializeField]
    private Button button;

    public Action<FacebookFriendDataSO> OnSelect;

    private FacebookFriendDataSO friendData;

    public void Init(FacebookFriendDataSO _friendData)
    {
        friendData = _friendData;
        profileImage.sprite = friendData.profileImage;
        nameText.text = friendData.nameText;

        OnSelect += (x) => SetSelectPanel(true);
        button.onClick.AddListener(() => { OnSelect?.Invoke(friendData); });   
    }

    public void SetSelectPanel(bool flag)
    {
        selectPanel.SetActive(flag);
    }
}
