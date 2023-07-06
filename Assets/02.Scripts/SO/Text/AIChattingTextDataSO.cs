using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/AIChatting")]
public class AIChattingTextDataSO : ScriptableObject
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
