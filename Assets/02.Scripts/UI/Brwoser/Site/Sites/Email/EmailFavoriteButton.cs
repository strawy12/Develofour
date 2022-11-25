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
    public Image fillStarImage;
    [SerializeField]
    private float colorDuraction = 0.7f;
    public UnityEvent OnClick { get { return button.onClick; } }

    public Action OnChangeMailType;

    [SerializeField]
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
        if (isChanging) return;
        isChanging = true;

        SetFavorited();

        OnChangeMailType?.Invoke();
        transform.parent.GetComponent<EmailLine>().emailCategory = EEmailCategory.Favorite;
        Sequence sequence = DOTween.Sequence(); 
        if(isFavorited)
        {   
            sequence.Append(fillStarImage.DOColor(new Color(0, 0, 0, 0), colorDuraction).OnComplete(() => isChanging = false));;
        }
        else
        {
            sequence.Append(fillStarImage.DOColor(Color.yellow, colorDuraction).OnComplete(() => isChanging = false));
        }
    }

    private void SetFavorited()
    {
        if(transform.parent.GetComponent<EmailLine>().emailCategory == EEmailCategory.Favorite)
        {
            isFavorited = true;
        }
        else
        {
            isFavorited = false;
        }
    }
}
