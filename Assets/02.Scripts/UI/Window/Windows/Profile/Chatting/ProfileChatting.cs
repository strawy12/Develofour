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
    public EProfileChatting eChat;
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
    [SerializeField]
    private List<ChatData> chatDataList;
    private Dictionary<EProfileChatting, string> chatDataDictionary = new Dictionary<EProfileChatting, string>();

    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private Transform textParent;
    [SerializeField]
    private ScrollRect scroll;
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;

    void Start()
    {
        Debug.Log("디버그용 스타트");
        Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            AddText(EProfileChatting.Password);
        }
    }

    public void DictionaryToList()
    {
        foreach(var data in chatDataList)
        {
            chatDataDictionary.Add(data.eChat, data.script);
        }
    }

    public void AddText(EProfileChatting data)
    {
        if(!chatDataDictionary.ContainsKey(data))
        {
            Debug.Log("해당 ENUM에 대한 키값이 존재하지 않음");
        }
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
        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        DictionaryToList();
        SetScrollView();
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
}
