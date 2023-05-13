using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/Monolog")]
public class MonologTextDataSO : TextDataSO
{
    [SerializeField]
    private int textDataType;
    [SerializeField]
    private int callPriority;

    // 필요 없을 거 같은 거
    public int CallPriority => callPriority;

    /// <summary>
    /// 해당 이름은 인 게임 내 보여지는 이름입니다.
    /// 영어로 작성하지 말고 한글로 작성해주세요.
    /// </summary>
    public string monologName;

    public int TextDataType
    {
        get
        {
            return textDataType;
        }
        set
        {
            textDataType = value;
        }
    }

#if UNITY_EDITOR

    [ContextMenu("ToString")]
    public void DebugToString()
    {
        string result = "";
        foreach(var text in textDataList)
        {
            result += text.text;
            result += "\n";
            result += "\n";
        }
        Debug.Log(result);
    }

#endif
}
