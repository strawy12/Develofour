using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageDownloader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private DownloadButton btn;

    private StandaloneInputModule input;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("이미지 엔터");
        btn.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("이미지 엑시트");
        btn.gameObject.SetActive(false);
    }

}
