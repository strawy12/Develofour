using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NoticeOutline : MonoBehaviour
{
    [SerializeField]
    private float onDelay;
    [SerializeField]
    private float activeTIme;
    [SerializeField]
    private float offDelay;

    private bool isOn;
    private Image image;
    public void StartOutline()
    {
        image ??= GetComponent<Image>();
        if (isOn) return;
        Sequence sequence = DOTween.Sequence();
        isOn = true;
        sequence.Append((image.DOFade(1, onDelay)));
        sequence.AppendInterval(activeTIme);
        sequence.Append(image.DOFade(0, offDelay));
        sequence.AppendCallback(() => { isOn = false; });
    }


}
