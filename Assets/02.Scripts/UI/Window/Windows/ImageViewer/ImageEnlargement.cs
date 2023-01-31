using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageEnlargement : MonoBehaviour, IPointerClickHandler, IScrollHandler
{
    public float imageScale = 1f;

    private float interval = 0.25f;
    private float doubleClickedTime = -1.0f;

    private bool isEnlargement = false;

    [SerializeField]
    private Image image;

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - doubleClickedTime) < interval)
        {
            Debug.Log("더블크릭");
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
        float imageEnlargementScale = 1f;

        if(isEnlargement) // 확대라면 축소
        {
            imageEnlargementScale = imageScale;
            image.transform.localScale = Vector3.one * imageEnlargementScale;

            isEnlargement= false;
        }
        if(!isEnlargement) // 축소중이면 확대
        {
            image.transform.localScale = Vector3.one * imageEnlargementScale;

            isEnlargement = true;
        }

    }

    private void ImageHill()
    {
        
    }
}
