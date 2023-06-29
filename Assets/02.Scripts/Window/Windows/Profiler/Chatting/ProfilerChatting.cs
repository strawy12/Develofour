using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;

public enum EProfileChatting
{
    Email,
    Password,
}

[Serializable]
public struct ChatData
{
    public EAiChatData eChat;
    [TextArea]
    public string script;
};

public class ProfilerChatting : MonoBehaviour
{
    protected float currentValue;

    [SerializeField]
    protected TMP_Text textPrefab;
    [SerializeField]
    protected Transform textParent;
    [SerializeField]
    protected ScrollRect scroll;
    [SerializeField]
    protected RectTransform scrollrectTransform;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;
    public void Init()
    {
        Hide();
    }

    public void Show()
    {
        EventManager.StartListening(EProfilerEvent.ProfilerSendMessage, PrintText);
        AddSaveTexts();
        SetScrollView();//스크롤뷰 가장 밑으로 내리기;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        EventManager.StopListening(EProfilerEvent.ProfilerSendMessage, PrintText);

        gameObject.SetActive(false);
    }

    private void PrintText(object[] ps)
    {
        if (ps[0] == null || !(ps[0] is string))
        {
            return;
        }
        string msg = (ps[0] as string);
        CreateTextUI(msg);
    }

    private void AddSaveTexts()
    {
        List<string> list = DataManager.Inst.SaveData.aiChattingList;

        foreach (string data in list)
        {
            CreateTextUI(data);
        }
    }

    private TMP_Text CreateTextUI(string msg)
    {
        TMP_Text textUI = Instantiate(textPrefab, textParent);
        textUI.text = msg;

        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textUI.rectTransform);
        SetScrollView();
        textUI.gameObject.SetActive(true);
        SetLastWidth();

        return textUI;
    }

    protected void SetScrollView()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(ScrollCor());
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
            scroll.verticalNormalizedPosition = 0;
        }
    }

    protected IEnumerator ScrollCor()
    {
        yield return new WaitForSeconds(0.025f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    protected void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
    }


    protected void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.ProfilerSendMessage, PrintText);
    }
}
