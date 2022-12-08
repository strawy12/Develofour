using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FacebookPidPanel : MonoBehaviour
{
    //pid have to get PidText �ǵ��ؽ�Ʈ�� �־����
    public FacebookPidPanelDataSO pidDataSO;

        
    //TODO - need Current User Data ����� â������ ����Ÿ SO�� Ŭ������
        
    [Header("CurrentUserData")]
    [SerializeField]
    private Sprite currentUserImage;
    [SerializeField]
    private string currentUserName;

    [Header("ProfileData")]
    [SerializeField]
    private FacebookPidProfileData profile;
        
    [Header("PidContentsData")]
    [SerializeField]
    private FacebookPidContentsData contents;

    [Header("CommentData")]
    [SerializeField]
    private FacebookPidCommentParentData commentParent;

    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI likeText;
    [SerializeField]
    private TextMeshProUGUI commentText;
    [SerializeField]
    private Button commentButton;
    [SerializeField]
    private Button commentSendButton;

    private bool isImage = false;

    public void Init()
    {
        commentSendButton.onClick.AddListener(CommentSend);
        CreateComment();
        profile.profileImage.sprite = pidDataSO.profileImage;
        profile.nameText.text = pidDataSO.profileNameText;
        profile.timeText.text = pidDataSO.profileTimeText;
        contents.pidText.text = pidDataSO.pidText;
        likeText.text = $"���ƿ� {pidDataSO.likeCount}��";
        commentText.text = $"��� {pidDataSO.commentCount}��";
        if(pidDataSO.pidImage != null)
        {
            contents.pidImage.sprite = pidDataSO.pidImage;
            contents.pidImage.gameObject.SetActive(true);
            isImage = true;
        }
    }

    private void CreateComment()
    {
        Debug.Log(pidDataSO.commentList.Count);
        for(int i = 0; i < pidDataSO.commentList.Count; i++)
        {
            FacebookPidComment comment = Instantiate(commentParent.commentPrefab, commentParent.commentParent);
            comment.Init(pidDataSO.commentList[i]);
            comment.gameObject.SetActive(true);
        }
    }

    public void CommentSend()
    {
        if (commentParent.commentInputField.text == null)
        {
            return;
        }

        FacebookPidComment comment = Instantiate(commentParent.commentPrefab, commentParent.commentParent);
        comment.profileImage.sprite = currentUserImage;
        comment.profileNameText.text = currentUserName;
        comment.commentText.text = commentParent.commentInputField.text;
        commentParent.commentInputField.text = "";
    }

}
