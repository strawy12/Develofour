using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;

public class OverlayPanel : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage;
    private bool isEnabled;
    private bool isRed = false;

    [SerializeField]
    private UIShiny uiEffect;

    [SerializeField]
    private Color redColor = new Color(1f, 0, 0, 0.5f);
    [SerializeField]
    private Color yellowColor = new Color(1f, 1f, 0, 1f);

    [SerializeField]
    private float maxAlpha = 0.8f;
    [SerializeField]
    private float minAlpha = 0.5f;
    public void Awake()
    {
        SetActive(false);
    }


    public void SetActive(bool flag)
    {
        isEnabled = flag;
        this.gameObject.SetActive(flag);

        StopAllCoroutines();
        if (isEnabled)
        {
            uiEffect.Play();

            if (!isRed)
                StartCoroutine(HighlightCoroutine());
        }
    }

    public void Setting(bool isRed) // false = yellow  ,  true = red
    {
        this.isRed = isRed;
        if (isRed)
        {
            backgroundImage.color = redColor;
        }

        else
        {
            backgroundImage.color = yellowColor;
        }


    }

    private IEnumerator HighlightCoroutine()
    {
        backgroundImage.DOFade(maxAlpha, 0f);
        while (isEnabled)
        {
            backgroundImage.DOFade(minAlpha, 1.5f);
            yield return new WaitForSeconds(2f);
            backgroundImage.DOFade(maxAlpha, 1.5f);
            yield return new WaitForSeconds(2f);
        }
    }

    void OnDestroy()
    {
        DOTween.KillAll(false);
        StopAllCoroutines();
    }
}
