using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class FacebookPidComment : MonoBehaviour
{
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TMP_Text profileNameText;
    [SerializeField]
    private TMP_Text commentText;

    private RectTransform rectTransform;

    public RectTransform RectTrm
    {
        get
        {
            return rectTransform;
        }
    }
    public void Init(Sprite sprite, string name, string comment)
    {
        profileImage.sprite = sprite;
        profileNameText.text = name;
        commentText.text = comment;
    }

    public void Setting(FacebookPidCommentData data)
    {
        rectTransform = (RectTransform)transform;
        float newHieght = 0f;

        profileImage.sprite = data.profileImage;
        profileNameText.text = data.profileNameText;

        commentText.text = data.pidText;

        Vector2 size = commentText.rectTransform.sizeDelta;
        for (int i = 0; i < commentText.text.Length; i++)
        {
            if (commentText.text[i] == '\n')
            {
                size.y += 16.2f;
            }
        }
        commentText.rectTransform.sizeDelta = size;

        newHieght += commentText.rectTransform.sizeDelta.y;
        newHieght += profileNameText.rectTransform.sizeDelta.y;
        newHieght += 8f;
        
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHieght);
    }
}
