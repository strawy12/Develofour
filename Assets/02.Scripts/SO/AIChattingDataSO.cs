using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AIChat
{
    public Sprite sprite;
    [TextArea(5, 30)]
    public string text;
}

[CreateAssetMenu(menuName = "SO/AiChat/ChattingData")]
public class AIChattingDataSO : ScriptableObject
{
    //����� so ������ ���ϴϱ� ����׿�����
    [SerializeField]
    private string id;
    public string ID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(id))
                return;
            id = value;
        }
    }

    public string chatName;

    public bool isAddGuideButton;

    public List<AIChat> AIChatList;
}