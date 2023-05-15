using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoTextData_",menuName ="SO/Profile/ProfileInfo/InfoText")]
public class ProfileInfoTextDataSO : ScriptableObject
{
    public int id;
    public string key;
    public string infomationText;
    public string noticeText;
    [SerializeField]
    private string categoryString;

    private EProfileCategory _category = EProfileCategory.None;
    public EProfileCategory category
    {
        get
        {
            if(_category == EProfileCategory.None)
            {
                _category = (EProfileCategory)System.Enum.Parse(typeof(EProfileCategory), categoryString);
            }

            return _category;
        }
    }
}
