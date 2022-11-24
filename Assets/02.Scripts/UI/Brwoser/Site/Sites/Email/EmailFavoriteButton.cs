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

    public Action OnChangeMailType;

    private bool isFavorited =false;
    private bool isChanging = false;

    public void Init(bool isFavorited)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(FavoriteOn);
        this.isFavorited = isFavorited;
    } 

    private void FavoriteOn()
    {
        Debug.Log(isFavorited);
        if (isChanging) return;
        isChanging = true;
        OnChangeMailType?.Invoke();
        Sequence sequence = DOTween.Sequence(); 
        if(isFavorited)
        {
            isFavorited = false;
            sequence.Append(fillStarImage.DOColor(Color.yellow, colorDuraction).OnComplete(() => isChanging = false));
        }
        else
        {
            isFavorited = true;
            sequence.Append(fillStarImage.DOColor(new Color(0,0,0,0), colorDuraction).OnComplete(() => isChanging = false));
        }
    } 
}
