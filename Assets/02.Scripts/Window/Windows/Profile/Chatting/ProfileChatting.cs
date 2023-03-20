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
    private Button OpenCloseButton;
    [SerializeField]
    private GameObject showImage;
    [SerializeField]
    private GameObject hideImage;
    [SerializeField]
    private float hideValue;
    [SerializeField]
    private float showValue;

    [SerializeField]
    private GameObject loadingPanel;

    private float currentValue;

    [SerializeField]
    private float moveDuration;
    private bool isMoving;
    private RectTransform movePanelRect;

    [Header("채팅관련")]

    private Dictionary<EAiChatData, string> chatDataDictionary = new Dictionary<EAiChatData, string>();

    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private Transform textParent;
    [SerializeField]
    private ScrollRect scroll;
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;

    [SerializeField]
    private ProfileChattingSaveSO SOData;
    
    public void Init()
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;

        EventManager.StartListening(EProfileEvent.SendMessage, AddText);

        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        DictionaryToList();
        GetSaveSetting();
        SetScrollView();
    }

    public void DictionaryToList()
    {
        foreach(var data in SOData.chatDataList)
        {
            chatDataDictionary.Add(data.eChat, data.script);
        }
    }
        
    public void AddText(string str)
    {   

        CreateChattingPanel(str);
    }

    public void AddText(object[] ps)
    {
        EWindowType type = EWindowType.ProfileWindow;
        EventManager.TriggerEvent(EWindowEvent.AlarmSend, new object[1] { type });

        if (!(ps[0] is EAiChatData))
        {
            if(ps[0] is string)
            {
                AddText(ps[0] as string);
            }
            return;
        }

        EAiChatData data = (EAiChatData)ps[0];

        if(!chatDataDictionary.ContainsKey(data))
        {
            Debug.Log("해당 ENUM에 대한 키값이 존재하지 않음");
        }

        CreateChattingPanel(chatDataDictionary[data]);

    }

    private void CreateChattingPanel(string str)
    {
        GameObject obj = Instantiate(textPrefab, textParent);
        obj.GetComponent<TMP_Text>().text = ">> " +str ;
        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
        SetScrollView();
        obj.gameObject.SetActive(true);
        SetLastWidth();
    }
    private void GetSaveSetting()
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

    public void ShowPanel()
    {
        Debug.Log("asdf");
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

    public void HidePanel()
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

    private void SetScrollView()
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

    private IEnumerator ScrollCor()
    {
        yield return new WaitForSeconds(0.025f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    private void SetWidths()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        foreach (var temp in rects)
        {
            temp.sizeDelta = new Vector2(currentValue - 60, 0);
        }
    }
    private void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
    }
    public void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.SendMessage, AddText);
    }
    public void OnApplicationQuit()
    {
        Debug.Log("디버그 용으로 Profile의 ChatDataSaveSO의 값을 매번 초기화 시키고 있습니다.");
        SOData.saveList.Clear();
    }
}
