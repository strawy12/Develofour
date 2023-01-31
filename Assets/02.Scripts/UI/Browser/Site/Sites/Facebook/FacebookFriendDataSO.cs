using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Facebook/FriendDataSO")]
public class FacebookFriendDataSO : ScriptableObject
{
    public Sprite backgroundImage;
    public Sprite profileImage;
    public string nameText;
    public string friendCountText;
    public List<FacebookPidPanelDataSO> pidList;

    [Header("Information")]
    public string highSchool;
    public string universal;
    public string phoneNumber;
    public int birthYear;
    public int birthMonth;
    public int birthDay;
    public string lovePerson;
}
