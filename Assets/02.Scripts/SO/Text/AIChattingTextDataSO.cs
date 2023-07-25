using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AIChat
{
    public Sprite sprite;
    public float sizeY;
    [TextArea(5, 30)]
    public string text;
}

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/AIChatting")]
public class AIChattingTextDataSO : ResourceSO
{
    public string chatName;
    [SerializeField]
    private List<AIChat> aiChatList;
    public List<AIChat> AIChatList => aiChatList;

    public AIChat this[int index]
    {
        get
        {
            return aiChatList[index];
        }
        set
        {
            if (index > aiChatList.Count - 1)
            {
                aiChatList.Add(value);
            }
            aiChatList[index] = value;
        }
    }

#if UNITY_EDITOR
    public void TextListSetting(List<AIChat> stringList)
    {
        aiChatList = stringList;
    }
#endif

}
