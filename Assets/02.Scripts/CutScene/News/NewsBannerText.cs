using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsBannerText : MonoBehaviour
{
    private TMP_Text currentText;
    private ContentSizeFitter contentSizeFitter;
    
    public RectTransform rectTransform
    {
        get
        {
            currentText ??= GetComponent<TMP_Text>();   

            return currentText.rectTransform;
        }
    }

    public void Init()
    {
        currentText = GetComponent<TMP_Text>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();

    }

    public void SetText(string textMsg)
    {
        currentText.SetText(textMsg);
        contentSizeFitter.SetLayoutHorizontal();
    }
}
