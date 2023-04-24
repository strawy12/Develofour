using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EMonologTextDataType
{
    None,
    WindowLoginComplete,
    Profile,
    StartMonolog,
    TutorialMonolog1,
    TutorialMonolog2,
    GuideLog1,
    OnUSBFileMonoLog,
    NotebookLoginGuideLog,
    SuspectResidence,
    SuspectRelationWithVictimGuide,
    StartSearchTutoMonolog,
    VictimUniversity,
    StartCutSceneMonolog1,
    StartCutSceneMonolog2,
    InstallCompleteMonoLog,
    EndProfileTutorial,
    TutorialNotFindName,
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
