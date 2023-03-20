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

    public void DictionaryToList()
    {
        foreach(var data in SOData.chatDataList)
        {
            chatDataDictionary.Add(data.eChat, data.script);
        }
    }
        
    public void AddText(string str)
    {
        foreach (var save in SOData.saveList)
        {
            if (save == str)
            {
                Debug.Log("동일한 데이터");
                return;
            }
        }
        
        NoticeSystem.OnNotice.Invoke("AI에게서 메세지가 도착했습니다!", str, 0, true, null, ENoticeTag.AIAlarm);

        SOData.saveList.Add(str);
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
        EventManager.TriggerEvent(EWindowEvent.AlarmSend, new object[1] { EWindowType.ProfileWindow });

        if (ps[0] is string)
        {
            AddText(ps[0] as string);
        }
        return;
    }



    public void Init()
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        EventManager.StartListening(EProfileEvent.SendMessage, AddText);

        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);

        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        DictionaryToList();
        GetSaveSetting();
        SetScrollView();
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

    public void AddSaveText(string data)
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

    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.L))
    //    {
    //        AddText(new object[1] { "안녕하세요" });
    //    }
    //    if(Input.GetKeyDown(KeyCode.K))
    //    {
    //        AddText(new object[1] { EAiChatData.Email });
    //    }    
    //}

    //윈도우를 오픈할때마다 실행해줘야함
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

    public void OnApplicationQuit()
    {
        Debug.Log("디버그 용으로 Profile의 ChatDataSaveSO의 값을 매번 초기화 시키고 있습니다.");
        SOData.saveList.Clear();
    }
}
