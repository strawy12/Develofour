using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinderCallWindow : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = transform as RectTransform; 
            }

            return _rectTransform;
        }
    }

    
    public virtual void SizeDoTween()
    {
        float minDuration = 0.16f;
        transform.localScale = new Vector2(0.9f, 0.9f);
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOScale(1, minDuration));
    }
}
