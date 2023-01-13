using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscordMessageImagePanel : MonoBehaviour
{
    [SerializeField]
    private Image msgImage;

    private RectTransform rectTransform;

    public Vector2 SizeDelta
    {
        get
        {
            rectTransform ??= GetComponent<RectTransform>();
            return rectTransform.sizeDelta;
        }
    }


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    public void SettingImage(Sprite sprite, Vector2 sizeVector)
    {
        msgImage.rectTransform.sizeDelta = sizeVector;
        msgImage.sprite = sprite;
        PanelSizeSetting();
    }

    public void SettingImage(Sprite sprite)
    {
        Debug.Log($"x:{sprite.bounds.size.x}. y:{sprite.bounds.size.y}");
        msgImage.rectTransform.sizeDelta = sprite.bounds.size * 30;
        msgImage.sprite = sprite;
        PanelSizeSetting();
    }

    public void SettingSizeDelta(Vector2 sizeVector)
    {
        msgImage.rectTransform.sizeDelta = sizeVector;
    }

    public void Release()
    {
        rectTransform ??= GetComponent<RectTransform>();
        msgImage.sprite = null;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0f);
        gameObject.SetActive(false);
    }

    private void PanelSizeSetting()
    {
        rectTransform ??= GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, msgImage.rectTransform.sizeDelta.y);

    }
}
