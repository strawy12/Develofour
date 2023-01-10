using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class FacebookPidComment : MonoBehaviour
{
    public Image profileImage;
    public TextMeshProUGUI profileNameText;
    public TextMeshProUGUI commentText;

    public void Init(Sprite sprite, string name, string comment)
    {
        profileImage.sprite = sprite;
        profileNameText.text = name;
        commentText.text = comment;
    }

    public void Init(FacebookPidCommentData data)
    {
        profileImage.sprite = data.profileImage;
        profileNameText.text = data.profileNameText;
        commentText.text = data.pidText;
    }
}
