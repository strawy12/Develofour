using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FacebookPidPanel : MonoBehaviour
{
    //pid have to get PidText 피드텍스트는 있어야함
    public FacebookPidPanelDataSO pidDataSO;

    //TODO - need Current User Data 현재는 창작자의 데이타 SO든 클래스든
    [SerializeField]
    private Sprite currentUserImage;
    [SerializeField]
    private string currentUserName;
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TextMeshProUGUI profileNameText;
    [SerializeField]
    private TextMeshProUGUI profileTimeText;
    [SerializeField]
    private TextMeshProUGUI pidText;
    [SerializeField]
    private Image pidImage;
    [SerializeField]
    private TextMeshProUGUI likeText;
    [SerializeField]
    private TextMeshProUGUI commentText;
    [SerializeField]
    private Button commentButton;
    [SerializeField]
    private Button commentSendButton;

    [SerializeField]
    private FacebookPidComment commentPrefab;
    [SerializeField]
    private Transform commentParent;
    [SerializeField]
    private TMP_InputField commentInputField;

    [SerializeField]
    private ContentSizeFitter csf;

    [SerializeField]
    private ContentSizeFitter commentcsf;

    private bool isImage = false;

    public void Init()
    {
        commentSendButton.onClick.AddListener(CommentSend);
        CreateComment();
        profileImage.sprite = pidDataSO.profileImage;
        profileNameText.text = pidDataSO.profileNameText;
        profileTimeText.text = pidDataSO.profileTimeText;
        pidText.text = pidDataSO.pidText;
        likeText.text = $"좋아요 {pidDataSO.likeCount}명";
        commentText.text = $"댓글 {pidDataSO.commentCount}명";
        if(pidDataSO.pidImage != null)
        {
            pidImage.sprite = pidDataSO.pidImage;
            pidImage.gameObject.SetActive(true);
            isImage = true;
        }
    }

    private void CreateComment()
    {
        Debug.Log(pidDataSO.commentList.Count);
        for(int i = 0; i < pidDataSO.commentList.Count; i++)
        {
            FacebookPidComment comment = Instantiate(commentPrefab, commentParent);
            comment.Init(pidDataSO.commentList[i]);
            comment.gameObject.SetActive(true);
        }
    }

    public void CommentSend()
    {
        if (commentInputField.text == null)
        {
            return;
        }

        FacebookPidComment comment = Instantiate(commentPrefab, commentParent);
        comment.profileImage.sprite = currentUserImage;
        comment.profileNameText.text = currentUserName;
        comment.commentText.text = commentInputField.text;
        commentInputField.text = "";

        commentcsf.enabled = true;
        commentcsf.SetLayoutHorizontal();
        commentcsf.SetLayoutVertical();
        commentcsf.enabled = false;
    }

}
