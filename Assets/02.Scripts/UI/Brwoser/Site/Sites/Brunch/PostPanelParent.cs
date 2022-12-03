using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostPanelParent : MonoBehaviour
{
    [SerializeField]
    private RectTransform blogPostPanelRect;
    [SerializeField]
    private int offsetY;

    private RectTransform rectTransform;

    public void Init(int count)
    {
        rectTransform ??= GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, offsetY + blogPostPanelRect.sizeDelta.y * count);
    }
}
