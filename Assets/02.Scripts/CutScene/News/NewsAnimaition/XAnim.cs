using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XAnim : MonoBehaviour
{
    [SerializeField]
    private RectTransform leftX;

    [SerializeField]
    private RectTransform rightX;

    [SerializeField]
    private float duration;
    [SerializeField]
    private float delay;
    [SerializeField]
    private Vector2 maxSize = new Vector2(75f,600f);

    [SerializeField]
    private Ease ease;
    [ContextMenu("Draw")]
    public float DrawAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(leftX.DOSizeDelta(new Vector2(maxSize.x, maxSize.y), duration).SetEase(ease));
        seq.Insert(delay, rightX.DOSizeDelta(new Vector2(maxSize.x, maxSize.y), duration).SetEase(ease));
        return duration + (duration - delay);
    }

    [ContextMenu("Erase")]
    public void EraseAnim()
    {
        leftX.sizeDelta = new Vector2(maxSize.x, 0);
        rightX.sizeDelta = new Vector2(maxSize.x, 0);
    }
}
