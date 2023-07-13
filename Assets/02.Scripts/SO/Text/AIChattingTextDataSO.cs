using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/AIChatting")]
public class AIChattingTextDataSO : ResourceSO
{
    private List<string> textList;

    public string this[int index]
    {
        get
        {
            return textList[index];
        }
        set
        {
            if (index > textList.Count - 1)
            {
                textList.Add(value);
            }
            textList[index] = value;
        }
    }

#if UNITY_EDITOR
    public void TextListSetting(List<string> stringList)
    {
        textList = stringList;
    }
#endif
}
