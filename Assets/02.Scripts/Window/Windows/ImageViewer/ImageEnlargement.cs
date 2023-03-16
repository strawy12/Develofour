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
    public float defaultScale;
    private float imageScale;

    public float maxImageScale = 4f;

    public float zoomSpeed = 0.1f;

    private Image currentImage;
    private TMP_Text imagePercentText;

    private int imageCurrentValue = 0;

    private float imageEnlargementScale = 2f;

    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    private bool isEnlargement = false;

    private int enlargementArrIndex = 0;
    private float[] enlargementArr = new float[] { 1f, 1.25f, 1.5f, 1.75f, 2f };

    private readonly Vector2 MAXSIZE = new Vector2(1173.333f, 660f);

    private const float RATIO = 1.636363636363636f;

    [ContextMenu("SetDegbugSize")]
    public void SetDegbugSize()
    {
        Image image = GetComponent<Image>();
        Vector2 size = image.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        image.rectTransform.sizeDelta = size;
    }

    public void Init(TMP_Text imagePercentText)
    {
        this.imagePercentText = imagePercentText;
        currentImage = GetComponent<Image>();

        transform.parent.GetComponent<ScrollRect>().content = transform as RectTransform;
        SetImageSizeReset();
        ReSetting();
    }
    public void SetImageSizeReset()
    {
        Vector2 size = currentImage.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        currentImage.rectTransform.sizeDelta = size;

        float scale = 1f;
        if (size.y > MAXSIZE.y)
        {
            scale = MAXSIZE.y / size.y;
        }
        else if (size.x > MAXSIZE.x)
        {
            scale = MAXSIZE.x / size.x;
        }
        transform.localScale = Vector3.one * scale;

        imageScale = transform.localScale.x;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
        currentImage.transform.localScale = Vector3.one * defaultScale;
        RenewalImageText();
    }

    public void OnScroll(PointerEventData eventData)
    {
        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
        Vector3 enlarScale = currentImage.transform.localScale + delta;

        Debug.Log(enlarScale);
        Debug.Log(imageScale);

        if (enlarScale.x <= imageScale)
        {
            return;
        }

        if (enlarScale.x >= maxImageScale)
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
            currentImage.transform.localScale = Vector3.one * imageEnlargementScale;
            currentImage.rectTransform.localPosition = imagePos;

            isEnlargement = true;
        }

        RenewalImageText();
    }

    public void EnlargementButtonClick()
    {
        enlargementArrIndex++;
        if (enlargementArrIndex >= 5)
        {
            enlargementArrIndex -= 1;
            return;
        }


        float enlarImageScale = imageScale * enlargementArr[enlargementArrIndex];
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    public void ReductionButtonClick()
    {
        enlargementArrIndex--;
        if (enlargementArrIndex <= -1)
        {
            enlargementArrIndex += 1;
            return;
        }

        float enlarImageScale = imageScale * enlargementArr[enlargementArrIndex];
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    private void RenewalImageText()
    {
        imageCurrentValue = (int)Mathf.Round(currentImage.transform.localScale.x * 100);

        imagePercentText.SetText(imageCurrentValue + "%");
    }
}
