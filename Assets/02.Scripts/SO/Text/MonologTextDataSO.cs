using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EMonologTextDataType
{
    None,
    USBMonolog,
    Profile,
    StartMonolog,
    StartNextMonolog,
    TutorialMonolog1,
    TutorialMonolog2,
    GuideLog1,
    OnUSBFileMonoLog,
    GuestLoginGuideLog,
    SuspectResidence,
    SuspectRelationWithVictimGuide,
    Count
}

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/Monolog")]
public class MonologTextDataSO : TextDataSO
{
    [SerializeField]
    private EMonologTextDataType textDataType;

    public EMonologTextDataType TextDataType
    {
        get
        {
            return textDataType;
        }
    }
}
