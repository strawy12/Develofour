using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBackground : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject rightMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        WindowManager.Inst.SelectedObjectNull();
        
        rightMenu.gameObject.SetActive(false);
    }
}
