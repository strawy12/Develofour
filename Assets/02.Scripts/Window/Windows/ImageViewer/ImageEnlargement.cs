using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ImageEnlargement : MonoBehaviour, IPointerClickHandler, IScrollHandler
{
    public Action enlargementClick;
    public Action reductionClick;

    private float imageScale = 1;

    public float maxImageScale = 50f;

    public float zoomSpeed = 0.1f;

    public float selectScale =0f;

    private Image currentImage;

    private TMP_Text imagePercentText;

    private int imageCurrentValue = 0;
    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    private bool isEnlargement = false;

    private readonly Vector2 MAXSIZE = new Vector2(1280f, 670f);

    public bool isDiscord;

#if UNITY_EDITOR
    [ContextMenu("SetDegbugSize")]
    public void SetImageSizeDelta()
    {
        currentImage  = GetComponent<Image>();
#else
    public void SetImageSizeDelta()
    {
#endif
        Vector2 size = currentImage.sprite.rect.size;

        if (size.x > MAXSIZE.x || size.y > MAXSIZE.y)
        {
            float x1 = size.x;
            float y1 = size.y;
            if (x1 > y1)
            {
                size.x = MAXSIZE.x;
                size.y = y1 * size.x / x1;
            }
            else
            {
                size.y = MAXSIZE.y;
                size.x = x1 * size.y / y1;
            }
        }

        currentImage.rectTransform.sizeDelta = size;
    }

    public void Init(TMP_Text imagePercentText, Sprite sprite)
    {
        this.imagePercentText = imagePercentText;
        currentImage = GetComponent<Image>();
        currentImage.sprite = sprite;

        transform.parent.GetComponent<ScrollRect>().content = transform as RectTransform;

        enlargementClick += EnlargementButtonClick;
        reductionClick += ReductionButtonClick;

        SetImageSizeReset();

        ReSetting();
    }

    public void Init(bool _isDiscord)
    {
        currentImage = GetComponent<Image>();
        isDiscord = _isDiscord;
    }

    public void SetImageSizeReset()
    {
        if (selectScale > 0f)
        {
            transform.localScale = Vector3.one * selectScale;
            imageScale = transform.localScale.x;
            return;
        }
        Vector2 size = currentImage.sprite.rect.size;
        
        SetImageSizeDelta();

        float scale = 1f;

        if (size.x > MAXSIZE.x || size.y > MAXSIZE.y)
        {
            if (size.y >= size.x)
            {
                scale = MAXSIZE.y / size.y;
            }
            else
            {
                scale = MAXSIZE.x / size.x;
            }
        }

        transform.localScale = Vector3.one * scale;

        imageScale = transform.localScale.x;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDiscord)
            return;

        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f;

            ImageDoubleClick(eventData.position);
        }
        else
        {
            doubleClickedTime = Time.time;
        }
    }

    public void ReSetting()
    {
        currentImage.transform.localScale = Vector3.one * imageScale;
        RenewalImageText();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (isDiscord)
            return;

        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        Vector3 enlarScale = currentImage.transform.localScale + delta;

        if (enlarScale.x <= Mathf.Min(imageScale, 1f))
        {
            return;
        }

        if (enlarScale.x > maxImageScale)
        {
            return;
        }

        currentImage.transform.localScale += delta;

        RenewalImageText();
    }

    private void ImageDoubleClick(Vector3 mousePos)
    {
        Vector3 imagePos = Vector3.zero;
        imagePos.x = -(mousePos.x - 760);
        imagePos.y = -(mousePos.y - 540);

        if (isEnlargement) // 확대라면 축소
        {
            currentImage.transform.localScale = Vector3.one * imageScale;
            currentImage.rectTransform.localPosition = Vector3.zero;

            isEnlargement = false;
        }
        else if (!isEnlargement) // 축소중이면 확대
        {
            currentImage.transform.localScale = Vector3.one * maxImageScale;
            currentImage.rectTransform.localPosition = imagePos;

            isEnlargement = true;
        }

        RenewalImageText();
    }

    public void EnlargementButtonClick()
    {
        float enlarImageScale = currentImage.transform.localScale.x * 1.1f;
        if (enlarImageScale > maxImageScale) return;
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    public void ReductionButtonClick()
    {
        float enlarImageScale = currentImage.transform.localScale.x / 1.1f;
        if (enlarImageScale < imageScale) return;
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    private void RenewalImageText()
    {
        imageCurrentValue = (int)Mathf.Round(currentImage.transform.localScale.x * 100);
        imagePercentText.SetText(imageCurrentValue + "%");
    }

}
