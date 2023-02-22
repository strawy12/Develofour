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
        SOData.saveList.Add(str);
        GameObject obj = Instantiate(textPrefab, textParent);
        obj.GetComponent<TMP_Text>().text = ">> " + str;
        obj.gameObject.SetActive(true);
        SetScrollView();
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

        foreach (var save in SOData.saveList)
        {
            if (save == data.ToString())
            {
                Debug.Log("동일한 데이터");
                return;
            }
        }
        
        SOData.saveList.Add(data.ToString());
        GameObject obj = Instantiate(textPrefab, textParent);
        obj.GetComponent<TMP_Text>().text = ">> " + chatDataDictionary[data];
        obj.gameObject.SetActive(true);
        SetScrollView();
    }

    //윈도우를 오픈할때마다 실행해줘야함
    private void SetScrollView()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    public void Init()
    {
        //스크롤뷰 가장 밑으로 내리기;
        EventManager.StartListening(EProfileEvent.SendMessage, AddText);

        
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
        if (isMoving) return;
        isMoving = true;
        movePanelRect.DOAnchorPosX(showValue, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            hideImage.SetActive(true);
            showImage.SetActive(false);
            isMoving = false;
        }); 
    }

    public void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        movePanelRect.DOAnchorPosX(hideValue, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            hideImage.SetActive(false);
            showImage.SetActive(true);
            isMoving = false;
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

    public void OnApplicationQuit()
    {
        Debug.Log("디버그 용으로 Profile의 ChatDataSaveSO의 값을 매번 초기화 시키고 있습니다.");
        SOData.saveList.Clear();
    }
}
