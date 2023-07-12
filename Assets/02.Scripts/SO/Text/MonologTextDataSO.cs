using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextData
{
    [TextArea(5, 30)]
    public string text;
    public Color textColor = Color.white;
}

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/Monolog")]
public class MonologTextDataSO : ResourceSO
{
    public string ID
    {
        get
        {
            return id;
        }
        set
        {
            if (string.IsNullOrEmpty(id)) return;
            id = value;
        }
    }
    [SerializeField]
    private List<TextData> textDataList;
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

#if UNITY_EDITOR

    [ContextMenu("ToString")]
    public void DebugToString()
    {
        string result = "";
        foreach (var text in textDataList)
        {
            result += text.text;
            result += "\n";
            result += "\n";
        }
        Debug.Log(result);
    }

#endif
}
