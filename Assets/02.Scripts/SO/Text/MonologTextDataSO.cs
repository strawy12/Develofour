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

    public int CallPriority => callPriority;

    public int TextDataType
    {
        get
        {
            return textDataType;
        }
    }

}
