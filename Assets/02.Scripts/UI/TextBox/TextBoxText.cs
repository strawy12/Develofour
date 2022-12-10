using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxText : MonoBehaviour
{
    private TMP_Text messageText;
    private ContentSizeFitter contentSizeFitter;

    public string text => messageText.text;
    public int maxVisibleCharacters
    {
        get => messageText.maxVisibleCharacters;
        set => messageText.maxVisibleCharacters = value;
    }

    public RectTransform rectTransform
    {
        get => messageText.rectTransform;
    }

    private void Awake()
    {
        messageText = GetComponent<TMP_Text>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();
    }

    public void SetText(string text)
    {
        messageText.SetText(text);
        SizeConversion();
    }

    public void SizeConversion()
    {
        contentSizeFitter.SetLayoutHorizontal();
        contentSizeFitter.SetLayoutVertical();
    }
}