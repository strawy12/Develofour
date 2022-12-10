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

    //SO �޾ƾ���
    private FacebookFriendDataSO data;

    [SerializeField]
    private FacebookPidPanel pidPanelPrefab;

    [SerializeField]
    private Transform pidParent;

    public void Init(FacebookFriendDataSO _data)
    {
        data = _data;
        profileImage.sprite = data.profileImage;
        nameText.text = data.nameText;
        infoText.text = data.infoText;
        friendAddButton.onClick.AddListener(() => FriendAdd());
        sendMessageButton.onClick.AddListener(() => SendMessage());

        for(int i = 0; i < data.pidList.Count; i++)
        {
            FacebookPidPanel pid = Instantiate(pidPanelPrefab, pidParent);
            
            pid.Init(data.pidList[i]);
            //content size fitter �ð�
            pid.gameObject.SetActive(true);
        }
    }

    private void FriendAdd()
    {
        Debug.Log("ģ�� �߰� ��ư");
    }

    private void SendMessage()
    {
        Debug.Log("�޼����� ������ٶ�����");
    }
}
