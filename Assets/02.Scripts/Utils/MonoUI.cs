using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MonoUI : MonoBehaviour
{
    protected CanvasGroup canvasGroup = null;
    [HideInInspector]
    public bool isFlag = false;

    public void SetActive(bool isActive)
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (gameObject.activeSelf == false) { gameObject.SetActive(true); }

        canvasGroup.alpha = isActive ? 1f : 0f;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
        isFlag = isActive;
    }

    [ContextMenu("SetActiveTrue")]
    private void SetActiveTrue()
    {
        SetActive(true);
    }

    [ContextMenu("SetActiveFalse")]
    private void SetActiveFalse()
    {
        SetActive(false);
    }
}
