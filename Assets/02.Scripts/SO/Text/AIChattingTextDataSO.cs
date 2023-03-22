using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EAIChattingTextDataType
{
    None,
    StartAIChatting,
    CompleteProfileAIChatting,
    StartNextAiChatting,
    SuspectIsLivingWithVictimGuide,
    SuspectIsLivingWithVictimHint,
    SuspectResidenceGuide,
    SuspectRelationWithVictimGuide,
    SuspectInfoComplete,
    Count
}

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/AIChatting")]
public class AIChattingTextDataSO : TextDataSO
{
    [SerializeField]
    private EAIChattingTextDataType textDataType;

    public EAIChattingTextDataType TextDataType
    {
        get
        {
            return textDataType;
        }
    }
}
