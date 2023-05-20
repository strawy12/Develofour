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

    private float imageScale;

    public float maxImageScale = 4f;

    public float zoomSpeed = 0.1f;

    private Image currentImage;

    private TMP_Text imagePercentText;

    private int imageCurrentValue = 0;
    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    private bool isEnlargement = false;

    private float[] enlargementArr = new float[] { 1f, 2f, 3f, 4f, 5f };

    private readonly Vector2 MAXSIZE = new Vector2(1280f, 670f);
    [SerializeField]
    private float MINSIZEX = 704f;
    private const float RATIOX = 1.63f;
    private const float RATIOY = 1.636363636363636f;
    public bool isDiscord;

    [ContextMenu("SetDegbugSize")]
    public void SetDegbugSize()
    {
        Image image = GetComponent<Image>();
        Vector2 size = image.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIOX;
        size.y /= RATIOY; 

        image.rectTransform.sizeDelta = size;
    }

    public void Init(TMP_Text imagePercentText)
    {
        this.imagePercentText = imagePercentText;
        currentImage = GetComponent<Image>();

        transform.parent.GetComponent<ScrollRect>().content = transform as RectTransform;

        enlargementClick += EnlargementButtonClick;
        reductionClick += ReductionButtonClick;

        SetImageSizeReset(MAXSIZE, MINSIZEX);

        ReSetting();
    }

    public void Init(bool _isDiscord)
    {
        currentImage = GetComponent<Image>();
        isDiscord = _isDiscord;
    }

    public void ChangeImage(Sprite sprite)
    {
        //VideoPlayer
        currentImage = GetComponent<Image>();
        currentImage.sprite = sprite;

        SetImageSizeReset(new Vector2(850, 850f), 500f);
        currentImage.transform.localScale = Vector3.one * imageScale;

    }

    

    public void SetImageSizeReset(Vector2 MAXSIZE, float MINSIZEX)
    {
        Vector2 size = currentImage.sprite.rect.size;

        size.x /= RATIOX;
        size.y /= RATIOY;

        currentImage.rectTransform.sizeDelta = size;

        float scale = 1f;

        if (size.y > MAXSIZE.y)
        {
            scale = MAXSIZE.y / size.y; 
        }
        else if(size.x > MAXSIZE.x)
        {
            scale = MAXSIZE.x / size.x; 
        }
        else if(size.x <= MINSIZEX)
        {
            scale = MINSIZEX / size.x;
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
            currentImage.transform.localScale = Vector3.one * maxImageScale;
            currentImage.rectTransform.localPosition = imagePos;

            isEnlargement = true;
        }

        RenewalImageText();
    }

    public void EnlargementButtonClick()
    {
        int idx = SetArrIndex(true);
        if (idx < 0 || idx > 5) { return; }
        float enlarImageScale = imageScale * enlargementArr[idx];
        if (enlarImageScale > maxImageScale) return;
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    public void ReductionButtonClick()
    {
        int idx = SetArrIndex(false);
        if (idx - 2 < 0 || idx - 2 > 5) { return; }
        float enlarImageScale = imageScale * enlargementArr[idx - 2];
        if (enlarImageScale < imageScale) return;
        currentImage.transform.localScale = Vector3.one * enlarImageScale;

        RenewalImageText();
    }

    private void RenewalImageText()
    {
        imageCurrentValue = (int)Mathf.Round(currentImage.transform.localScale.x * 100);

        imagePercentText.SetText(imageCurrentValue + "%");
    }

    private int SetArrIndex(bool isEnlar)
    {
        int num = imageCurrentValue;
        int result = num / 100;

        if (!isEnlar)
        {
            if (num % 100 != 0)
            {
                result += 1;
            }
        }
        return result;
    }
#if UNITY_EDITOR
    [ContextMenu("GetSize")]
    public void GetSize()
    {
        currentImage = GetComponent<Image>();

        Vector2 size = currentImage.sprite.rect.size;

        size.x /= RATIOX;
        size.y /= RATIOY;

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
        else if (size.x <= MINSIZEX)
        {
            scale = MINSIZEX / size.x;
        }

        transform.localScale = Vector3.one * scale;

        imageScale = transform.localScale.x;
    }

#endif
}
