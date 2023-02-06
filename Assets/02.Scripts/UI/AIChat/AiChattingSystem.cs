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
            Debug.LogError("얻어온 text의 데이터가 없습니다");
            return;
        }

        // 이거 주석 지우고 UI에 넣어주면 됨
        /* 
        var noticeList = noticePanelQueue.Where((x) => x.HeadText == data.Head);
        if (noticeList.Count() >= 1)
        {
            Debug.Log("이미 있는 알람임");
            return;
        }
        StartCoroutine(NoticeCoroutine(data, delay));
        */
    }
}
