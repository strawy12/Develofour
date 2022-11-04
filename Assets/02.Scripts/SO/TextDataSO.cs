using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ETextDataType
{
    None,
    News,
} 
[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [SerializeField]
    private ETextDataType textDataType;

    [SerializeField]
    [TextArea(1,10)]
    private List<string> textDataList;
    
    public ETextDataType GetTextDataType {
        get 
        {
            return textDataType;
        } 
    }

    public string this[int index] 
    { 
        get
        {
            return textDataList[index];
        } 
    }
}
