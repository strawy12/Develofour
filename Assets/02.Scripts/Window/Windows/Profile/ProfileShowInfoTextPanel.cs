using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProfileShowInfoTextPanel : MonoBehaviour
{
    public TMP_Text text;

    public GameObject showPanelParent;

    public TMP_Text downText;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    public RectTransform RectTrm
    {
        get
        {
            rectTransform ??= GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public void SetDownText()
    {
        parentRect ??= showPanelParent.GetComponent<RectTransform>();
        downText.text = text.text;
        parentRect.sizeDelta = downText.rectTransform.sizeDelta;
    }
}
