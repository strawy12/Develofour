using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoliceGameSendButton : MonoBehaviour
{
    private RectTransform rectTransform;
    public void SuccessEffect()
    {
        rectTransform ??= GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPosY(20, 0.2f));
        seq.Join(rectTransform.DOLocalRotate(new Vector3(0, 360, 0), 0.8f));
        seq.Join(rectTransform.DOScale(1.2f, 0.5f)).OnComplete(() => rectTransform.DOScale(1f, 0.3f));
        seq.Insert(0.6f, rectTransform.DOAnchorPosX(0, 0.2f));
    }
}
