using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageEnlargement : MonoBehaviour, IPointerClickHandler, IScrollHandler
{
    public float imageScale = 1f;

    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            doubleClickedTime = -1.0f;

            ImageDoubleClick();
        }
        else
        {
            doubleClickedTime = Time.time;
        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    private void ImageDoubleClick()
    {

    }
}
