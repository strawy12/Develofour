using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;

public class EmailFavoriteButton : MonoBehaviour
{
    private Button button;

    [SerializeField]
    private Image starImage;
    [SerializeField]
    private Image fillStarImage;
    [SerializeField]
    private float colorDuraction = 0.7f;
    public UnityEvent OnClick { get { return button.onClick; } }
    private bool isFavorited =false;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(FavoriteOn);
    }

    public void Init(bool isFavorited)
    {
        this.isFavorited = isFavorited;
    } 

    private void FavoriteOn()
    {
        Sequence sequence = DOTween.Sequence(); 
        if(isFavorited)
        {
            isFavorited = false;
            sequence.Append(fillStarImage.DOColor(Color.yellow, colorDuraction));
        }
        else
        {
            isFavorited = true;
            sequence.Append(fillStarImage.DOColor(new Color(0,0,0,0), colorDuraction));
        }
    } 
}
