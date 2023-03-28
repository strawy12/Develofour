using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName ="TextData_",menuName = "SO/TextDataSO")]
public class TextDataSO : ScriptableObject
{
    // 이름을 통해서 타입 구분
    // 변수로 타입 구분을 하느냐 SO 나눈 거 같아
    // 변수로 구분을 하게 만드는게 맞고
    // TextDataSO 고정 

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
