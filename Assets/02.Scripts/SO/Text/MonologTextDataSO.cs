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

    // �ʿ� ���� �� ���� ��
    public int CallPriority => callPriority;

    /// <summary>
    /// �ش� �̸��� �� ���� �� �������� �̸��Դϴ�.
    /// ����� �ۼ����� ���� �ѱ۷� �ۼ����ּ���.
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
