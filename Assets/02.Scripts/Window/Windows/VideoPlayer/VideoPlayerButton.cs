using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoPlayerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button; 

    public GameObject highLightImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        highLightImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highLightImage.SetActive(false);
    }

}
