using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [TextArea(5, 30)]
    public List<string> textDataList;
    public string this[int index]
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
