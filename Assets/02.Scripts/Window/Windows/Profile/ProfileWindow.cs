using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ProfileWindow : Window
{
    [Header("ProfilerPanel")]
    [SerializeField]
    private ProfilePanel profilePanel;
    [SerializeField]
    private InfoCheckPanel infoCheckPanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;

    [Header("ProfilerChatUI")]
    [SerializeField]
    private ProfileChatting profileChatting;

    [Header("ProfilerBar")]
    [SerializeField]
    private Button profileSystemBtn;
    [SerializeField]
    private Button infoCheckBtn;
    [SerializeField]
    private Button fileSearchBtn;

    [SerializeField]
    private TMP_Text profilingText;
    [SerializeField]
    private TMP_Text infoCheckText;
    [SerializeField]
    private TMP_Text fileSearchText;


    [Header("UIEtc")]
    [SerializeField]
    private Button moveBtn;
    [SerializeField]
    private RectTransform area;

    [SerializeField]
    private float moveDelay = 0.75f;

    private bool isOpen = true;
    private bool isMoving = false;

    private bool isFirstTutorial;

    private Button beforeClickButton;

    protected override void Init()
    {
        base.Init();

        profileChatting.Init();
        profilePanel.Init();
        fileSearchPanel.Init();

        profileSystemBtn.onClick?.AddListener(OnClickShowProfilingBtn);
        infoCheckBtn.onClick?.AddListener(OnClickShowInfo);
        fileSearchBtn.onClick?.AddListener(OnClickShowFileSearch);

        moveBtn.onClick.AddListener(delegate { StartCoroutine(HideAllPanel()); });

        TutorialStart();
    }

    private void OnClickShowProfilingBtn()
    {
        if (beforeClickButton == profileSystemBtn)
        {
            return;
        }

        Color blackColor = new Color(0, 0, 0, 255);
        Color whiteColor = new Color(255, 255, 255, 255);

        profileSystemBtn.image.color = blackColor;
        profilingText.color = whiteColor;

        infoCheckBtn.image.color = whiteColor;
        infoCheckText.color = blackColor;

        fileSearchBtn.image.color = whiteColor;
        fileSearchText.color = blackColor;

        beforeClickButton = profileSystemBtn;

        StartCoroutine(OnShowProfile());
    }

    private void OnClickShowInfo()
    {
        if (beforeClickButton == infoCheckBtn)
        {
            return;
        }

        Color blackColor = new Color(0, 0, 0, 255);
        Color whiteColor = new Color(255, 255, 255, 255);

        infoCheckBtn.image.color = blackColor;
        infoCheckText.color = whiteColor;

        profileSystemBtn.image.color = whiteColor;
        profilingText.color = blackColor;

        fileSearchBtn.image.color = whiteColor;
        fileSearchText.color = blackColor;

        beforeClickButton = infoCheckBtn;

        StartCoroutine(OnShowInfo());
    }

    private void OnClickShowFileSearch()
    {
        if (beforeClickButton == fileSearchBtn)
        {
            return;
        }

        Color blackColor = new Color(0, 0, 0, 255);
        Color whiteColor = new Color(255, 255, 255, 255);

        fileSearchBtn.image.color = blackColor;
        fileSearchText.color = whiteColor;

        infoCheckBtn.image.color = whiteColor;
        infoCheckText.color = blackColor;

        profileSystemBtn.image.color = whiteColor;
        profilingText.color = blackColor;

        beforeClickButton = fileSearchBtn;

        StartCoroutine(OnShowFileSearch());
    }

    private void TutorialStart()
    {
        if (DataManager.Inst.SaveData.isTutorialStart == false)
        {
            EventManager.TriggerEvent(ETutorialEvent.TutorialStart, new object[0]);
            EventManager.StartListening(ETutorialEvent.ProfileMidiumStart, StartGuideMinimumBtn);
            DataManager.Inst.SaveData.isTutorialStart = true;
        }
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
        if (!isMoving)
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

        if (!isMoving)
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
    private void StartGuideMinimumBtn(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke((RectTransform)windowBar.MinimumBtn.transform);
        EventManager.StartListening(ETutorialEvent.ProfileMidiumEnd, TutotrialClickMinimumBtn);
    }
    private void TutotrialClickMinimumBtn(object[] ps)
    {
        GuideUISystem.EndGuide?.Invoke();
        EventManager.StopListening(ETutorialEvent.ProfileMidiumEnd, TutotrialClickMinimumBtn);
        EventManager.StopListening(ETutorialEvent.ProfileMidiumStart, StartGuideMinimumBtn);
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
    }
    public override void WindowMinimum()
    {
        base.WindowMinimum();
        EventManager.TriggerEvent(ETutorialEvent.ProfileMidiumEnd);

    }
}
