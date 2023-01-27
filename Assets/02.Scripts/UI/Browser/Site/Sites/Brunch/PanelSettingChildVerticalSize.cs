using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSettingChildVerticalSize : MonoBehaviour
{
    [SerializeField]
    private RectTransform blogPostPanelRect;
    [SerializeField]
    private int offsetY;
    [SerializeField]
    private float spacing = 0;
    private RectTransform rectTransform;

    public void ChangeVerticalUICount(int count)
    {
        rectTransform ??= GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, offsetY + spacing * count + blogPostPanelRect.sizeDelta.y * count);
    }
}
