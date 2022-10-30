using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Rendering;

public class StartBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Value")]
    [SerializeField]
    private float duration = 0.2f;
    [SerializeField]
    private float highLightOpacity = 0.5f;
    [SerializeField]
    private float iconIntensity = 1f;

    [Header("Bind")]
    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private StartAttributePanel startAttributePanel;

    private Image currentImage;



    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ActiveAttributePanel();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighLightBtn(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighLightBtn(false);
    }

    private void ActiveAttributePanel()
    {
        startAttributePanel.Show();
    }

    private void HighLightBtn(bool isHighLight)
    {
        Color iconColor = new Color(iconIntensity, iconIntensity, iconIntensity);

        currentImage.DOFade(isHighLight ? highLightOpacity : 0f, duration);
        iconImage.DOColor(isHighLight ? iconColor : Color.white, duration);
    }
}
