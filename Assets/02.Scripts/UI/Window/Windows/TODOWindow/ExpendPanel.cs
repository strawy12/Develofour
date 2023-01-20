using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExpendPanel : MonoBehaviour
{
    [SerializeField]
    private HighlightBtn closeBtn;
    [SerializeField]
    private HighlightBtn beforeBtn;
    [SerializeField]
    private HighlightBtn nextBtn;

    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text statusText;
    [SerializeField]
    private TMP_Text categoryText;
    [SerializeField]
    private TMP_Text successRateText;
    [SerializeField]
    private TMP_Text bodyText;

    private TodoData todoData;


    public void ShowExpendPanel()
    {
        transform.DOKill();
        transform.DOScaleX(1f, 0.25f);
    }

    public void HideExpendPanel()
    {
        transform.DOKill();
        transform.DOScaleX(0f, 0.25f);
    }

}
