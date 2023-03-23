using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuidePanel : MonoBehaviour
{

    [Header("������ ����")]
    [SerializeField]
    protected Button OpenCloseButton;
    [SerializeField]
    protected GameObject showImage;
    [SerializeField]
    protected GameObject hideImage;
    [SerializeField]
    protected float hideValue;
    [SerializeField]
    protected float showValue;

    [SerializeField]
    protected GameObject loadingPanel;

    protected float currentValue;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;
    [SerializeField]
    protected float moveDuration;
    protected bool isMoving;
    protected RectTransform movePanelRect;


    [Header("���̵� ����")]
    [SerializeField]
    private ProfileGuideButtonParent guideParent;

    private ProfileChattingSaveSO chattingSaveSO;

    public void Init(ProfileChattingSaveSO saveSO)
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //��ũ�Ѻ� ���� ������ ������;
        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        chattingSaveSO = saveSO;


    }

    protected void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(0, hideValue), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = hideValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            showImage.SetActive(true);
            hideImage.SetActive(false);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    protected void ShowPanel()
    {

        Debug.Log("ShowPanel");
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(0, showValue), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            showImage.SetActive(false);
            hideImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
        });

    }


}
