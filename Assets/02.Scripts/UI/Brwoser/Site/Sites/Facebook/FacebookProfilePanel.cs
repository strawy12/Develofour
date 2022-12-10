using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FacebookProfilePanel : MonoBehaviour
{
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private Image backgroundImage;

    public RectTransform rect;

    [Header("Buttons")]
    [SerializeField]
    private Button friendAddButton;
    [SerializeField]
    private Button sendMessageButton;

    //SO 받아야함
    private FacebookFriendDataSO data;

    [SerializeField]
    private FacebookPidPanel pidPanelPrefab;

    [SerializeField]
    private Transform pidParent;

    //피드의 최대값
    [SerializeField]
    private List<FacebookPidPanel> pidList;

    public void Setting(FacebookFriendDataSO _data)
    {
        data = _data;
        for (int i = 0; i < pidList.Count; i++)
        {
            pidList[i].gameObject.SetActive(false);
        }
        profileImage.sprite = data.profileImage;
        nameText.text = data.nameText;
        infoText.text = data.infoText;
        backgroundImage.sprite = data.backgroundImage;
        //friendAddButton.onClick.AddListener(() => FriendAdd());
        //sendMessageButton.onClick.AddListener(() => SendMessage());

        for (int i = 0; i < data.pidList.Count; i++)
        {
            pidList[i].Setting(data.pidList[i]);
            pidList[i].gameObject.SetActive(true);
        }
    }

    private void FriendAdd()
    {
        Debug.Log("친구 추가 버튼");
    }

    private void SendMessage()
    {
        Debug.Log("메세지가 허락해줄때까지");
    }
}
