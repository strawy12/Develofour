using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class ProfileWindow : Window
{
    [Header("ProfilerPanel")]
    [SerializeField]
    private ProfilePanel profilePanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;

    [Header("ProfilerChatUI")]
    [SerializeField]
    private ProfileChatting profileChatting;

    [SerializeField]
    private Button infoPanelBtn;
    [SerializeField]
    private Button searchPanelBtn;

    [SerializeField]
    private List<TMP_Text> btnTextList;

    [Header("UIEtc")]
    [SerializeField]
    private Button moveBtn;
    [SerializeField]
    private RectTransform area;

    [SerializeField]
    private float moveDelay = 0.75f;

    private bool isOpen = true;
    private bool isMoving = false;

    //DataManager
    private bool isOpenFileSearch;
    private bool isOpenInfoCheck;

    private Button beforeClickButton;
    private Color blackColor;
    private Color whiteColor;


    protected override void Init()
    {
        base.Init();

        profileChatting.Init();
        profilePanel.Init();
        fileSearchPanel.Init();

        infoPanelBtn.onClick?.AddListener(OnClickShowProfilingBtn);
        searchPanelBtn.onClick?.AddListener(OnClickShowFileSearch);
        CheckingButton();

        moveBtn.onClick.AddListener(delegate { StartCoroutine(HideAllPanel()); });
        EventManager.StartListening(EProfileSearchTutorialEvent.GuideSearchButton, GuideSearchButton);
        TutorialStart();

        EventManager.StartListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
        blackColor = new Color(0, 0, 0, 255);
        whiteColor = new Color(255, 255, 255, 255);

        beforeClickButton = infoPanelBtn;
    }

    private void CheckingButton()
    {
        if (isOpenFileSearch)
        {
            searchPanelBtn.interactable = true;
        }
        else
        {
            searchPanelBtn.interactable = false;
        }

        if (isOpenInfoCheck)
        {
            infoPanelBtn.interactable = true;
        }
        else
        {
            infoPanelBtn.interactable = false;
        }
    }

    private void CheckProfilerOnOff(object[] emptyPs)
    {
        if (profilePanel.gameObject.activeSelf == false)
        {
            OnClickShowProfilingBtn();
        }
    }

    private void OnClickShowProfilingBtn()
    {

        if (beforeClickButton == infoPanelBtn)
        {
            isOpen = false;
            ShowProfileCategoryPanel();
            return;
        }

        beforeClickButton = infoPanelBtn;

        StartCoroutine(OnShowProfile());
    }

    private void OnClickShowFileSearch()
    {
        if (beforeClickButton == searchPanelBtn)
        {
            isOpen = false;
            ShowFileSearchPanel();
            return;
        }
        beforeClickButton = searchPanelBtn;

        StartCoroutine(OnShowFileSearch());
    }

    private void ButtonBlackSetting()
    {

    }

    private void TutorialStart()
    {
        if (DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) == false)
        {
            EventManager.TriggerEvent(ETutorialEvent.TutorialStart, new object[0]);
            EventManager.StartListening(ETutorialEvent.ProfileMidiumStart, StartGuideMinimumBtn);
            DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);
        }
    }

    private IEnumerator HideAllPanel()
    {
        isMoving = true;
        area.DOAnchorPosY(-1000, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            profilePanel.gameObject.SetActive(false);
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
        ShowPanel(profilePanel.gameObject);
    }

    private void ShowFileSearchPanel()
    {
        ShowPanel(fileSearchPanel.gameObject);
    }

    private void ShowPanel(GameObject panel)
    {
        isMoving = true;
        panel.gameObject.SetActive(true);

        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isOpen = true;
            isMoving = false;
            if (GameManager.Inst.GameState == EGameState.Tutorial && DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
            {
                EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchInputPanel);
            }
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

    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
    }
    private void GuideSearchButton(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(btnList[2].transform as RectTransform);
    }
}
