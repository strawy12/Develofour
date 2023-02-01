using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FacebookPidCommentParentData : MonoBehaviour
{
    public FacebookPidComment commentPrefab;
    public Transform commentParent;
    public TMP_InputField commentInputField;

    private RectTransform rectTransform;

    public void Setting(float height)
    {
        rectTransform = (RectTransform)transform;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
