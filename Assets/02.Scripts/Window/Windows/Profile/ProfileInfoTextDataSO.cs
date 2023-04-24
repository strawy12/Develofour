using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoTextData_",menuName ="SO/Profile/ProfileInfo/InfoText")]
public class ProfileInfoTextDataSO : ScriptableObject
{
    public string key;
    public string infomationText;
    public string noticeText;
    public EProfileCategory category;
}
