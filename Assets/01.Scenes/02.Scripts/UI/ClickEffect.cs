using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickEffect : MonoBehaviour
{
    private Image clickImage;
    private RectTransform rectTransform;
    public void Click()
    {
        clickImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = Define.CanvasMousePos;

        clickImage.DOFade(0f, 0.5f).SetEase(Ease.InCirc);
        transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.InCirc).OnComplete(() =>
            {
                //clickImage.DOFade(1f, 0f);
                //gameObject.SetActive(false);
                transform.localScale = (Vector3.zero);
                Destroy(gameObject);
            });
        });
    }
}
