using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoliceGameSendButton : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField]
    private float scale = 1.2f;
    [SerializeField]
    private Vector2 targetPos = new Vector2(415.5f, 20f);
    [SerializeField]
    private float rotateDuration = 0.8f;
    [SerializeField]
    private float scaleDuration = 0.3f;
    [SerializeField]
    private float posDuration = 0.2f;
    public void SuccessEffect()
    {
        rectTransform ??= GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOAnchorPosY(targetPos.y, posDuration));
        seq.Join(rectTransform.DORotate(new Vector3(0, 360, 0), rotateDuration, RotateMode.FastBeyond360));
        seq.Join(rectTransform.DOScale(scale, scaleDuration));
        seq.Append(rectTransform.DOScale(1f, scaleDuration));

        seq.Insert(0.6f, rectTransform.DOAnchorPosX(targetPos.x, posDuration));
    }
}
