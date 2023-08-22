using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OverlayPanel : MonoBehaviour
{
    public Image yellowImage;
    public Image redImage;
    private bool isEnable;

    public void Awake()
    {
        yellowImage.DOFade(0, 1.5f).SetLoops(-1, LoopType.Yoyo);
        SetActive(false);
    }


    public void SetActive(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    public void Setting(bool flag) // false = yellow  ,  true = red
    {
        yellowImage.gameObject.SetActive((flag == false) ? true : false);
        redImage.gameObject.SetActive((flag == true) ? true : false);
    }

    void OnDestroy()
    {
        DOTween.KillAll(false);
    }
}
