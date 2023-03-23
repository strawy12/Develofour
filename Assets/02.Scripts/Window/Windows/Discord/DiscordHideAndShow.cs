using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class DiscordHideAndShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TextMeshProUGUI hideText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hideText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideText.gameObject.SetActive(false);
    }

}
