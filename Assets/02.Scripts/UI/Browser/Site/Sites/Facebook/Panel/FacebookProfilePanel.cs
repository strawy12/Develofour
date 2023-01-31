using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FacebookProfilePanel : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI friendCountText;

    [Header("InfoText")]
    public TextMeshProUGUI highSchoolText;
    public TextMeshProUGUI universalText;
    public TextMeshProUGUI phoneNumberText;
    public TextMeshProUGUI birthText;
    public TextMeshProUGUI lovePersonText;

    //SO �޾ƾ���
    private FacebookFriendDataSO data;

    [SerializeField]
    private FacebookPidPanel pidPanelPrefab;

    [SerializeField]
    private Transform pidParent;

    //�ǵ��� �ִ밪
    [SerializeField]
    private List<FacebookPidPanel> pidList;

    public FacebookProfileSetHeight profileRect;

    public void Setting(FacebookFriendDataSO _data)
    {
        data = _data;
        for (int i = 0; i < pidList.Count; i++)
        {
            pidList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < data.pidList.Count; i++)
        {
            pidList[i].Setting(data.pidList[i]);
            pidList[i].gameObject.SetActive(true);
        }

        profileImage.sprite = data.profileImage;
        nameText.text = data.nameText;
        friendCountText.text = data.friendCountText;

        if(string.IsNullOrEmpty(data.highSchool))
        {
            highSchoolText.text = "�� ǥ���� ����б� ���� ����";
        }
        else
        {
            highSchoolText.text = "��" + data.highSchool;
        }

        if (string.IsNullOrEmpty(data.universal))
        {
            universalText.text = "�� ǥ���� ���б� ���� ����";
        }
        else
        {
            universalText.text = "��" + data.universal;
        }

        if (string.IsNullOrEmpty(data.phoneNumber))
        {
            phoneNumberText.text = "�� ǥ���� ��ȭ��ȣ ���� ����";
        }
        else
        {
            phoneNumberText.text = "��" + data.phoneNumber;
        }

        if (data.birthYear == 0)
        {
            birthText.text = "�� ǥ���� ���� ���� ����";
        }
        else
        {
            string str = "�� ";
            str += data.birthYear.ToString() + ".";
            if(data.birthMonth >= 10)
                str += data.birthMonth.ToString() + ".";
            else
                str += "0" + data.birthMonth.ToString() + ".";

            if (data.birthDay >= 10)
                str += data.birthDay.ToString();
            else
                str += "0" + data.birthDay.ToString();

            birthText.text = str;
        }

        if (string.IsNullOrEmpty(data.lovePerson))
        {
            lovePersonText.text = "�� ǥ���� ���� ���� ���� ����";
        }
        else
        {
            lovePersonText.text = "�� ���� " + data.lovePerson + " ��(��) ������";
        }

        profileRect.Setting();
    }

}
