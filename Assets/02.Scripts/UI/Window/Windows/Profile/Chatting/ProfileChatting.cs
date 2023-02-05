using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileChatting : MonoBehaviour
{
    [Header("������ ����")]
    [SerializeField]
    private Button OpenCloseButton;

    [SerializeField]
    private GameObject showImage;
    [SerializeField]
    private GameObject hideImage;
    [SerializeField]
    private float hideValue;
    [SerializeField]
    private float showValue;
    [SerializeField]
    private float moveDuration;
    private bool isMoving;
    private bool isHide;

    private RectTransform rect;

    void Start()
    {
        Debug.Log("����׿� ��ŸƮ");
        Init();
    }

    public void Init()
    {
        //��ũ�Ѻ� ���� ������ ������;
        OpenCloseButton.onClick.AddListener(HidePanel);
        rect = GetComponent<RectTransform>();
    }
    
    public void ShowPanel()
    {
        if (isMoving) return;
        isMoving = true;
        rect.DOAnchorPosX(showValue, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            hideImage.SetActive(true);
            showImage.SetActive(false);
            isMoving = false;
        }); 
    }

    public void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        rect.DOAnchorPosX(hideValue, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            hideImage.SetActive(false);
            showImage.SetActive(true);
            isMoving = false;
        });
    }
}
