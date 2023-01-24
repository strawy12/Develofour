using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
public class DiscordInputField : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI text;
    public Action OnShowAccount;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnShowAccount?.Invoke();
    }
}
