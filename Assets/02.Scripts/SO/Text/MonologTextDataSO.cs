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

    /// <summary>
    /// 해당 이름은 인 게임 내 보여지는 이름입니다.
    /// 영어로 작성하지 말고 한글로 작성해주세요.
    /// </summary>

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

    private List<TextData> textDataList;
    public TextData this[int index]
    {
        get
        {
            return textDataList[index];
        }
        set
        {
            if (index > textDataList.Count - 1)
            {
                textDataList.Add(value);
            }
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


    public void TextListSetting(List<TextData> textDataList)
    {
        this.textDataList = textDataList;
    }
#endif
}
