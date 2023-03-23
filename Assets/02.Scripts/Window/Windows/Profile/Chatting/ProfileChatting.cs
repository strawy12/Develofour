using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

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

public class ProfileChatting : MonoBehaviour
{
    [Header("움직임 관련")]
    [SerializeField]
    protected Button OpenCloseButton;
    [SerializeField]
    protected GameObject showImage;
    [SerializeField]
    protected GameObject hideImage;
    [SerializeField]
    protected float hideValue;
    [SerializeField]
    protected float showValue;

    [SerializeField]
    protected GameObject loadingPanel;

    protected float currentValue;

    [SerializeField]
    protected float moveDuration;
    protected bool isMoving;
    protected RectTransform movePanelRect;

    [Header("채팅관련")]

    protected Dictionary<EAiChatData, string> chatDataDictionary = new Dictionary<EAiChatData, string>();

    [SerializeField]
    protected GameObject textPrefab;
    [SerializeField]
    protected Transform textParent;
    [SerializeField]
    protected ScrollRect scroll;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;

    [SerializeField]
    protected ProfileChattingSaveSO SOData;

    protected float currentDelay;

    [Header("가이드관련")]
    [SerializeField]
    private ProfileGuidePanel guidePanel;
    public void DictionaryToList()
    {
        foreach(var data in SOData.chatDataList)
        {
            chatDataDictionary.Add(data.eChat, data.script);
        }
    }
        
    public void AddText(string str)
    {
        
        GameObject obj = Instantiate(textPrefab, textParent);
        obj.GetComponent<TMP_Text>().text = ">> " + str;
        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
        SetScrollView();
        obj.gameObject.SetActive(true);
        SetLastWidth();
    }

    public void AddText(object[] ps)
    {
        if (ps[0] is string)
        {
            AddText(ps[0] as string);
        }
        return;
    }



    public virtual void Init()
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        ConnectEvent();

        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        guidePanel.Init(SOData);
        DictionaryToList();
        GetSaveSetting();
        SetScrollView();
    }

    protected virtual void ConnectEvent()
    {
        EventManager.StartListening(EProfileEvent.ProfileSendMessage, AddText);
    }

    public void GetSaveSetting()
    {
        foreach(var save in SOData.saveList)
        {
            AddSaveText(save);
        }
        SetScrollView();
        SetWidths();
    }

    private void AddSaveText(string data)
    {
        if (chatDataDictionary.ContainsKey(AIStringToEnum(data)))
        {
            GameObject obj = Instantiate(textPrefab, textParent);
            obj.GetComponent<TMP_Text>().text = ">> " + chatDataDictionary[AIStringToEnum(data)];
            obj.gameObject.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate(textPrefab, textParent);
            obj.GetComponent<TMP_Text>().text = ">> " + data;
            obj.gameObject.SetActive(true);
        }
    }

    EAiChatData AIStringToEnum(string str)
    {
        if (Enum.IsDefined(typeof(EAiChatData), str))
            return (EAiChatData)Enum.Parse(typeof(EAiChatData), str);
        else
            return EAiChatData.None;
    }

    protected virtual void ShowPanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(showValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(false);
            showImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
        });

    }

    protected virtual void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(hideValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = hideValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(true);
            showImage.SetActive(false);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    protected void SetScrollView()
    {
        if(this.gameObject.activeInHierarchy)
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

    protected void SetWidths()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        foreach (var temp in rects)
        {
            temp.sizeDelta = new Vector2(currentValue - 60, 0);
        }
    }
    protected void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
    }
    public void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.ProfileSendMessage, AddText);
    }
    public void OnApplicationQuit()
    {
        Debug.Log("디버그 용으로 Profile의 ChatDataSaveSO의 값을 매번 초기화 시키고 있습니다.");
        SOData.saveList.Clear();
    }
}
