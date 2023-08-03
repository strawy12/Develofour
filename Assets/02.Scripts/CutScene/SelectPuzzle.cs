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
        Fade(false);
    }

    public void Fade(bool value)
    {
        if(value)
        {
            canvasGroup.DOFade(1, 1);
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.DOFade(0, 1);
            canvasGroup.blocksRaycasts = true;
        }
    }
}
