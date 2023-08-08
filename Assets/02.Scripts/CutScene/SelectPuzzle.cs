using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SelectPuzzle : MonoBehaviour
{ 
    public SelectInfoTrigger selectInfoTrigger;

    public CanvasGroup canvasGroup;

    public void Init()
    {
        Fade(false, 0);
    }

    public void Fade(bool value, float time)
    {
        if(value)
        {
            canvasGroup.DOFade(1, time);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.DOFade(0, time);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}
