using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AiChattingSystem : MonoBehaviour
{
    public static Action<EAiChatData, float> OnGeneratedNotice;

    private void Awake()
    {
        Bind();
    }

    private void Start()
    {
        Init();
    }

    private void Bind()
    {
    }

    private void Init()
    {
        OnGeneratedNotice += AiChattingUpdate;

    }

    public TextDataSO GetTextData(EAiChatData aiChatDataType)
    {
        TextDataSO aiTextData = null;

        try
        {
            aiTextData = Resources.Load<TextDataSO>($"TextData/TextData_{aiChatDataType}");
        }
        catch (System.NullReferenceException e)
        {
            //Debug.Log($"NoticeData {noticeDataType} is null\n{e}");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        
        return aiTextData;
    }
    
    private void AiChattingUpdate(EAiChatData aiChatDataType, float delay)
    {
        TextDataSO data = GetTextData(aiChatDataType);
        if (data == null)
        {
            Debug.LogError("���� text�� �����Ͱ� �����ϴ�");
            return;
        }

        // �̰� �ּ� ����� UI�� �־��ָ� ��
        /* 
        var noticeList = noticePanelQueue.Where((x) => x.HeadText == data.Head);
        if (noticeList.Count() >= 1)
        {
            Debug.Log("�̹� �ִ� �˶���");
            return;
        }
        StartCoroutine(NoticeCoroutine(data, delay));
        */
    }
}
