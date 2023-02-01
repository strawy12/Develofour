using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FacebookPidPanel : MonoBehaviour
{
    //pid have to get PidText �ǵ��ؽ�Ʈ�� �־����
    private FacebookPidPanelDataSO pidDataSO;


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
    private TMP_Text likeText;
    [SerializeField]
    private TMP_Text commentText;
    [SerializeField]
    private Button commentButton;
    //[SerializeField]
    //private Button commentSendButton;

    private bool isCreate = false;

    private float commenntsHeight = 0;
    private RectTransform rectTransform;
    public void Setting(FacebookPidPanelDataSO _pidDataSO)
    {
        Debug.Log("Pid Setting");
        pidDataSO = _pidDataSO;
        //commentSendButton.onClick.AddListener(CommentSend);
        if (!isCreate) CreateComment();
        profile.profileImage.sprite = pidDataSO.profileImage;
        profile.nameText.text = pidDataSO.profileNameText;
        profile.timeText.text = pidDataSO.profileTimeText;
        likeText.text = $"���ƿ� {pidDataSO.likeCount}��";
        commentText.text = $"��� {pidDataSO.commentCount}��";

    }


    private void SetContent()
    {
        Debug.Log("setContent");
        rectTransform ??= GetComponent<RectTransform>();

        float newHieght = rectTransform.sizeDelta.y;
        commentParent.Setting(commenntsHeight);

        Debug.Log(commenntsHeight);
        newHieght += commenntsHeight;
        newHieght += contents.RectTrm.sizeDelta.y;
        newHieght += 100f;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHieght);

    }

    private void CreateComment()
    {
        Debug.Log("?");
        isCreate = true;
        commenntsHeight = 0f;
        for (int i = 0; i < pidDataSO.commentList.Count; i++)
        {
            FacebookPidComment comment = Instantiate(commentParent.commentPrefab, commentParent.commentParent);
            comment.Setting(pidDataSO.commentList[i]);
            comment.gameObject.SetActive(true);
            commenntsHeight += comment.RectTrm.sizeDelta.y;
        }

        contents.Setting(pidDataSO.pidText, SetContent, pidDataSO.pidImage);
    }

    //public void CommentSend()
    //{
    //    if (commentParent.commentInputField.text == null)
    //    {
    //        return;
    //    }

    //    FacebookPidComment comment = Instantiate(commentParent.commentPrefab, commentParent.commentParent);
    //    comment.profileImage.sprite = currentUserImage;
    //    comment.profileNameText.text = currentUserName;
    //    comment.commentText.text = commentParent.commentInputField.text;
    //    commentParent.commentInputField.text = "";
    //}

}
