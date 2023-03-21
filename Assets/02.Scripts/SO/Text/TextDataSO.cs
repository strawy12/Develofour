using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
public class TextDataSO : ScriptableObject
{
    [SerializeField]
    private List<TextData> textDataList;


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
