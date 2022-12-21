using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ETextDataType
{
    None,
    News,
}

[System.Serializable]
public class TextData
{
    public string name;
    public Color color;

    [TextArea(5, 30)]
    public string text; 
}

[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [SerializeField]
    private ETextDataType textDataType;

    [SerializeField]
    private List<TextData> textDataList;
    
    public ETextDataType TextDataType {
        get 
        {
            return textDataType;
        } 
    }

    public TextData this[int index]
    { 
        get
        {
            return textDataList[index];
        } 
    }

    public int Count
    {
        get
        {
            return (int)textDataList.Count;
        }
    }

}
