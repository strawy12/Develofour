using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageDownloader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private DownloadButton btn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.gameObject.SetActive(false);
    }

}
