using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YoutubeInteractionButton : MonoBehaviour
{
    [SerializeField] private bool isHateBtn;
    private bool isClicked = false;
    private float durationTime = 0.4f;
    private Button button;
    private Image image;
    private RectTransform rectTransform;
    public UnityEvent OnClick { get { return button.onClick; } }
    private Vector3 originScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.enabled = false;
        rectTransform = GetComponent<RectTransform>();
        originScale = rectTransform.localScale;
        button.onClick.AddListener(ClickEffect);
    }

    private void ClickEffect()
    {
        if (!isClicked)
        {
            isClicked = true;
            Sequence seq = DOTween.Sequence();
            //before
            seq.Append(rectTransform.DOScale(originScale * 1.3f, durationTime));
            if (isHateBtn)
            {
                image.enabled = true;
                seq.Join(image.DOColor(Color.yellow, durationTime));
            }
            seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - 1f, durationTime));
            seq.Join(rectTransform.DORotate(new Vector3(0, 0, 20), durationTime));

            //after
            seq.Append(rectTransform.DOScale(originScale, durationTime));
            if (isHateBtn)
            {
                seq.Join(image.DOColor(Color.white, durationTime));
            }
            seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 1f, durationTime));
            seq.Join(rectTransform.DORotate(new Vector3(0, 0, 0), durationTime));
            //end
            seq.AppendCallback(() => 
            { 
                image.enabled = false;
                isClicked = false;
            });
        }
    }
}
