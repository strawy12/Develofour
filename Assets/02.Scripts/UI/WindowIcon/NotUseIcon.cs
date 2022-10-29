using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotUseIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //[SerializeField]
    //private Image iconImage;
    //[SerializeField]
    //private TMP_Text iconNameText;

    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    private void Start()
    {
        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedImage.gameObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedImage.gameObject.SetActive(false);
        pointerStayImage.gameObject.SetActive(false);
    }
}