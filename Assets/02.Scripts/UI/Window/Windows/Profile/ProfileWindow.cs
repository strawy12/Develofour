using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ProfileWindow : Window
{
    [SerializeField]
    private ProfilePanel profilePanel;

    [SerializeField]
    private ProfileChatting profileChatting;
    [SerializeField]
    private InfoCheckPanel infoCheckPanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;
    [SerializeField]
    private Button profileSystemBtn;
    [SerializeField]
    private Button infoCheckBtn;
    [SerializeField]
    private Button fileSearchBtn;
    [SerializeField]
    private RectTransform area;
    [SerializeField]
    private float moveDelay = 0.75f;
    private bool isOpen = false;
    private bool isMoving = false;
    protected override void Init()
    {
        base.Init();
        profileChatting.Init();
        profilePanel.Init();
        profileSystemBtn.onClick.AddListener(delegate { StartCoroutine(OnShowProfile()); } );
        infoCheckBtn.onClick.AddListener(delegate { StartCoroutine(OnShowInfo()); });
        fileSearchBtn.onClick.AddListener(delegate { StartCoroutine(OnShowFileSearch()); });
    }
    private IEnumerator HideAllPanel()
    {
        area.DOAnchorPosY(-1000, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
           profilePanel.gameObject.SetActive(false);
           infoCheckPanel.gameObject.SetActive(false);
           fileSearchPanel.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(moveDelay + 0.05f);
    }

    private IEnumerator OnShowProfile()
    {
        if(!isMoving)
        {
            isMoving = true;
            if (isOpen)
            {
                yield return StartCoroutine(HideAllPanel());
            }

            ShowProfileCategoryPanel();
        }
    }

    private IEnumerator OnShowInfo()
    {
        if(!isMoving)
        {
            isMoving = true;
            if (isOpen)
            {
                yield return StartCoroutine(HideAllPanel());
            }

            ShowInfoCheckPanel();
        }
    }

    private IEnumerator OnShowFileSearch()
    {
        if(!isMoving)
        {
            isMoving = true;
            if (isOpen)
            {
                yield return StartCoroutine(HideAllPanel());
            }

            ShowFileSearchPanel();
        }
    }

    private void ShowProfileCategoryPanel()
    {
        profilePanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });
    }
    private void ShowInfoCheckPanel()
    {
        infoCheckPanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });
        
    }
    private void ShowFileSearchPanel()
    {
        fileSearchPanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });

    }
}
