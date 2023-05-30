using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
[System.Serializable]
public class TextData
{
    [TextArea(5, 30)]
    public string text;
    public Color textColor = Color.white;
}
public class TextDataSO : ScriptableObject
{
   
    
    public List<TextData> textDataList;
    public TextData this[int index]
    {
        get
        {
            return textDataList[index];
        }
        set 
        { 
            textDataList[index] = value; 
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
