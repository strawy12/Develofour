using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CallPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Button CallButton;
    [SerializeField]
    private PhoneCallUI phoneCallUI;

    private bool isEnter;

    void Start()
    {
        CallButton.onClick?.AddListener(OnPhoneCallUI);
        phoneCallUI.OnCloseIngnoreFlag += () => isEnter;
    }

    private void OnPhoneCallUI()
    {
        if (phoneCallUI.gameObject.activeSelf)
        {
            phoneCallUI.Close();
        }
        else
        {
            phoneCallUI.Open();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
    }
}
