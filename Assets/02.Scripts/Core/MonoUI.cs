using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MonoUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    public void SetActive(bool isActive)
    {
        canvasGroup ??= GetComponent<CanvasGroup>();

        canvasGroup.alpha = isActive ? 1f : 0f;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }

}
