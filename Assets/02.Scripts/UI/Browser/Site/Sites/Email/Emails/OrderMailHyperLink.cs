using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrderMailHyperLink : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public int fileId;
    public int directoryId;

    public void OnPointerClick(PointerEventData eventData)
    {
        FileManager.Inst.AddFile(fileId, directoryId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = Color.blue;
        text.fontStyle = FontStyles.Bold;
        text.fontStyle = FontStyles.Italic;
        text.fontStyle = FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = Color.black;
        text.fontStyle = 0;
    }

}
