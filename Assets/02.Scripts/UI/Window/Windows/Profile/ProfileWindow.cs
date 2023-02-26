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
    private Button moveBtn;
    [SerializeField]
    private RectTransform area;
    [SerializeField]
    private float moveDelay = 0.75f;
    private bool isOpen = true;
    private bool isMoving = false;

    private bool isFirstTutorial;
    private bool isSecondTutorial;

    protected override void Init()
    {
        base.Init();
        profileChatting.Init();
        profilePanel.Init();
        fileSearchPanel.Init();
        profileSystemBtn.onClick.AddListener(delegate { StartCoroutine(OnShowProfile()); } );
        infoCheckBtn.onClick.AddListener(delegate { StartCoroutine(OnShowInfo()); });
        fileSearchBtn.onClick.AddListener(delegate { StartCoroutine(OnShowFileSearch()); });
        moveBtn.onClick.AddListener(delegate { StartCoroutine(HideAllPanel()); });
    }
    private IEnumerator HideAllPanel()
    {
        isMoving = true;
        area.DOAnchorPosY(-1000, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
           profilePanel.gameObject.SetActive(false);
           infoCheckPanel.gameObject.SetActive(false);
           fileSearchPanel.gameObject.SetActive(false);
        });
        yield return new WaitForSeconds(moveDelay + 0.05f);
        isMoving = false;
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
        if (GameManager.Inst.GameState == EGameState.Tutorial) yield break;

        if (!isMoving)
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
        if (GameManager.Inst.GameState == EGameState.Tutorial) yield break;
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
        isMoving = true;
        profilePanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });
    }
    private void ShowInfoCheckPanel()
    {
        isMoving = true;
        infoCheckPanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });
        
    }
    private void ShowFileSearchPanel()
    {
        isMoving = true;
        fileSearchPanel.gameObject.SetActive(true);
        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
        });
    }

    public void ChattingAlarm(object[] ps)
    {

    }
}
