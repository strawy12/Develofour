using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DiscordFriendLine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Action<DiscordFriendLine> OnLeftClickPanel;
    public Action<Vector2, DiscordFriendLine> OnRightClickPanel;

    [SerializeField]
    private GameObject highlightPanel;

    public GameObject HighlightPanel
    {
        get => highlightPanel;
    }

    [SerializeField]
    private GameObject selectPanel;
    public GameObject SelectPanel
    {
        get => selectPanel;
    }

    [SerializeField]
    private GameObject noticePanel;
    public GameObject NoticePanel
    {
        get => noticePanel;
    }

    [SerializeField]
    private Image profileImage;
    public Image ProfileImage
    {
        get => profileImage;
        set { profileImage = value; }
    }

    [SerializeField]
    private TextMeshProUGUI nameText;
    public TextMeshProUGUI NameText
    {
        get => nameText;
        set { nameText = value; }
    }

    [SerializeField]
    private TextMeshProUGUI statusNameText;
    public TextMeshProUGUI StatusNameText
    {
        get => statusNameText;
        set { statusNameText = value; }
    }

    [SerializeField]
    private TextMeshProUGUI statusText;
    public TextMeshProUGUI StatusText
    {
        get => statusText;
        set { statusText = value; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightPanel.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClickPanel?.Invoke(eventData.position, this);
            //우클릭 창 만들기
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            selectPanel.SetActive(true);
            OnLeftClickPanel?.Invoke(this);
        }
    }

    public void QuitSelectPanel()
    {
        selectPanel.SetActive(false);
    }
}
