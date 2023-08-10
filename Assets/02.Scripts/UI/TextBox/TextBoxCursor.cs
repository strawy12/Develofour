using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxCursor : MonoBehaviour
{
    private Image currentImage;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float delay = 0.3f; 
    private bool isTurnOn;

    public void Init()
    {
        currentImage = GetComponent<Image>();
    }

    public void TurnOn()
    {
        if (isTurnOn) return;

        gameObject.SetActive(true);
        isTurnOn = true;
        StartCoroutine(FadeEffect());
    }

    public void TurnOff()
    {
        if (!isTurnOn) return;

        isTurnOn = false;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    private IEnumerator FadeEffect()
    {
        while(isTurnOn)
        {
            currentImage.DOFade(1f, duration);
            yield return new WaitForSeconds(delay);
            currentImage.DOFade(0f, duration);
            yield return new WaitForSeconds(delay);
        }
    }
}
