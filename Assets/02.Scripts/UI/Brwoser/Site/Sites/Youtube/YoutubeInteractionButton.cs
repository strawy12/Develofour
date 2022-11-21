using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YoutubeInteractionButton : MonoBehaviour
{
    [SerializeField] private bool isHateBtn;

    private bool isClicked = false;
    private bool isOverLapLike = false;
    private bool isOverLapHate = false;

    private float durationTime = 0.4f;

    [SerializeField]
    private Image otherImage;
    [SerializeField]
    private TMP_Text hatePeopleText;

    private Button button;
    private Image image;
    private RectTransform rectTransform;
    
    private Vector3 originScale;
    
    public UnityEvent OnClick { get { return button.onClick; } }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        
        image.enabled = false;
        originScale = rectTransform.localScale;
        
        button.onClick.AddListener(ClickEffect);
    }

    private void ClickEffect()
    {
        if(!isClicked)
        {
            isClicked = true;
            Sequence seq = DOTween.Sequence();

            //before
            seq.Append(rectTransform.DOScale(originScale * 1.2f, durationTime));

            if (isHateBtn)
            {
                if(!isOverLapHate)
                {
                    isOverLapHate = true;
                    image.enabled = true;
                    otherImage.enabled = false;

                    hatePeopleText.text = "73";

                    seq.Join(image.DOColor(Color.yellow, durationTime));
                    seq.Join(otherImage.DOColor(Color.white, durationTime));
                }
                else if(isOverLapHate)
                {
                    isOverLapHate = false;
                    image.enabled = false;
                    isClicked = false;

                    hatePeopleText.text = "72";

                    seq.Join(image.DOColor(Color.white, durationTime));
                    seq.Append(rectTransform.DOScale(originScale, durationTime));
                    return;
                }

            }
            if (!isHateBtn)
            {
                if(!isOverLapLike)
                {
                    isOverLapLike = true;
                    image.enabled = true;
                    otherImage.enabled = false;

                    hatePeopleText.text = "72";

                    seq.Join(image.DOColor(Color.red, durationTime));
                    seq.Join(otherImage.DOColor(Color.white, durationTime));
                }
                else if (isOverLapLike)
                {
                    isOverLapLike = false;
                    image.enabled = false;
                    isClicked = false;

                    seq.Join(image.DOColor(Color.white, durationTime));
                    seq.Append(rectTransform.DOScale(originScale, durationTime));
                    return;
                }
            }

            seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - 1f, durationTime));
            seq.Join(rectTransform.DORotate(new Vector3(0, 0, 20), durationTime));

            //after
            seq.Append(rectTransform.DOScale(originScale, durationTime));

            seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 1f, durationTime));
            seq.Join(rectTransform.DORotate(new Vector3(0, 0, 0), durationTime));

            //end
            seq.AppendCallback(() =>
            {
                isClicked = false;
            });
        }
    }
}
