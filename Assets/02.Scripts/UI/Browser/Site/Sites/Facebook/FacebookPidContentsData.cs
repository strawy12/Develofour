using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FacebookPidContentsData : MonoBehaviour
{
    [SerializeField]
    private Image pidImage;
    [SerializeField]
    private TextMeshProUGUI pidText;

    private RectTransform rectTransform;

    public delegate void SettingCallback();

    public RectTransform RectTrm
    {
        get
        {
            return rectTransform;
        }
    }
    public void Setting(string text, SettingCallback callback, Sprite sprite = null)
    {
        rectTransform ??= GetComponent<RectTransform>();
        float newHieght = 0f;
        if (sprite != null)
        {
            pidImage.sprite = sprite;
            newHieght += pidImage.rectTransform.sizeDelta.y;
            pidImage.gameObject.SetActive(true);
        }
        else
        {
            pidImage.gameObject.SetActive(false);
        }
        pidText.text = text;
        Vector2 size = pidText.rectTransform.sizeDelta;

        for (int i = 0; i < pidText.text.Length; i++)
        {
            if (pidText.text[i] == '\n')
            {
                size.y += 38;
            }
        }
        Debug.Log("size.y:" + size.y);
        pidText.rectTransform.sizeDelta = size;
        newHieght += pidText.rectTransform.sizeDelta.y;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHieght);
        callback();
    }
}
