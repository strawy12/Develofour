using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FacebookProfilePanel : MonoBehaviour
{
    public FacebookProfileDataSO DEBUGDATA;

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
    private FacebookProfileDataSO data;

    [SerializeField]
    private Transform pidParent;

    [SerializeField]
    private FacebookPidPanel pidPrefab;

    public FacebookProfileSetHeight profileRect;

    public void Setting(FacebookProfileDataSO _data)
    {
        data = _data;
        Transform[] childList = pidParent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
            Debug.Log("����");
        }

        for (int i = 0; i < data.pidList.Count; i++)
        {
            Debug.Log("����");
            FacebookPidPanel pid = Instantiate(pidPrefab, pidParent) as FacebookPidPanel;
            pid.Setting(data.pidList[i]);
            pid.gameObject.SetActive(true);
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
        gameObject.SetActive(true);
        profileRect.Setting();
    }

}
