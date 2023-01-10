using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiscordFriendLine : MonoBehaviour
{
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

}
