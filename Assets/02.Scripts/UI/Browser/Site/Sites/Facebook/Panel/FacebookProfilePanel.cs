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

    //SO 받아야함
    private FacebookFriendDataSO data;

    [SerializeField]
    private FacebookPidPanel pidPanelPrefab;

    [SerializeField]
    private Transform pidParent;

    //피드의 최대값
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
            highSchoolText.text = "● 표시할 고등학교 정보 없음";
        }
        else
        {
            highSchoolText.text = "●" + data.highSchool;
        }

        if (string.IsNullOrEmpty(data.universal))
        {
            universalText.text = "● 표시할 대학교 정보 없음";
        }
        else
        {
            universalText.text = "●" + data.universal;
        }

        if (string.IsNullOrEmpty(data.phoneNumber))
        {
            phoneNumberText.text = "● 표시할 전화번호 정보 없음";
        }
        else
        {
            phoneNumberText.text = "●" + data.phoneNumber;
        }

        if (data.birthYear == 0)
        {
            birthText.text = "● 표시할 생일 정보 없음";
        }
        else
        {
            string str = "● ";
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
            lovePersonText.text = "● 표시할 연애 상태 정보 없음";
        }
        else
        {
            lovePersonText.text = "● 현재 " + data.lovePerson + " 과(와) 연애중";
        }

        profileRect.Setting();
    }

}
