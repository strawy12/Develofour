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

    [Header("Buttons")]
    [SerializeField]
    private Button friendAddButton;
    [SerializeField]
    private Button sendMessageButton;

    [SerializeField]
    private FacebookProfilePanelDataSO data;

    [SerializeField]
    private Transform pidParent;

    public void Init()
    {
        profileImage.sprite = data.profileImage;
        nameText.text = data.nameText;
        infoText.text = data.infoText;
        friendAddButton.onClick.AddListener(() => FriendAdd());
        sendMessageButton.onClick.AddListener(() => SendMessage());

        for(int i = 0; i < data.pidList.Count; i++)
        {
            FacebookPidPanel pid = Instantiate(data.pidList[i], pidParent);
            pid.Init();
            pid.gameObject.SetActive(true);
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
