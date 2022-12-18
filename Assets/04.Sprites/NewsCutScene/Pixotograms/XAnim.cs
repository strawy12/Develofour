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
    private Ease ease;
    [ContextMenu("Draw")]
    public void DrawAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(leftX.DOSizeDelta(new Vector2(40, 330), duration).SetEase(ease));
        seq.Insert(delay, rightX.DOSizeDelta(new Vector2(40, 330), duration).SetEase(ease));
    }

    [ContextMenu("Erase")]
    public void EraseAnim()
    {
        leftX.sizeDelta = new Vector2(40, 0);
        rightX.sizeDelta = new Vector2(40, 0);
    }
}
