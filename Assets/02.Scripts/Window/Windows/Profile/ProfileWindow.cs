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
    private InfoCheckPanel infoCheckPanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;

    [Header("ProfilerChatUI")]
    [SerializeField]
    private ProfileChatting profileChatting;

    [SerializeField]
    private List<Button> btnList;
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

        btnList[0].onClick?.AddListener(OnClickShowInfo);
        btnList[1].onClick?.AddListener(OnClickShowProfilingBtn);
        btnList[2].onClick?.AddListener(OnClickShowFileSearch);
        CheckingButton();

        moveBtn.onClick.AddListener(delegate { StartCoroutine(HideAllPanel()); });

        TutorialStart();

        EventManager.StartListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StartListening(EProfileSearchTutorialEvent.GuideSearchButton, GuideSearchButton);
        blackColor = new Color(0, 0, 0, 255);
        whiteColor = new Color(255, 255, 255, 255);
        ButtonSetting("profileSystem");
        beforeClickButton = btnList[1];
    }

    private void CheckingButton()
    {
        if(isOpenFileSearch)
        {
            btnList[2].interactable = true;
        }
        else
        {
            btnList[2].interactable = false;
        }

        if(isOpenInfoCheck)
        {
            btnList[0].interactable = true;
        }
        else
        {
            btnList[0].interactable = false;
        }
    }

    private void CheckProfilerOnOff(object[] emptyPs)
    {
        if(profilePanel.gameObject.activeSelf == false)
        {
            OnClickShowProfilingBtn();
        }
    }

    private void OnClickShowProfilingBtn()
    {
        ButtonSetting("profileSystem");

        if (beforeClickButton == btnList[1])
        {
            isOpen = false;
            ShowProfileCategoryPanel();
            return;
        }

        beforeClickButton = btnList[1];

        StartCoroutine(OnShowProfile());
    }

    private void OnClickShowInfo()
    {

        ButtonSetting("infoCheck");

        if (beforeClickButton == btnList[0])
        {
            isOpen = false;
            ShowInfoCheckPanel();
            return;
        }

        beforeClickButton = btnList[0];

        StartCoroutine(OnShowInfo());
    }
    
    private void OnClickShowFileSearch()
    {
        ButtonSetting("fileSearch");

        if (beforeClickButton == btnList[2])
        {
            isOpen = false;
            ShowFileSearchPanel();
            return;
        }
        beforeClickButton = btnList[2];

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
        ButtonSetting();
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
        if(!DataManager.Inst.SaveData.isTutorialClear)
        {
            yield break;
        }
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

    private void ShowInfoCheckPanel()
    {
        ShowPanel(infoCheckPanel.gameObject);
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
            if (GameManager.Inst.GameState == EGameState.Tutorial && DataManager.Inst.SaveData.isTutorialClear)
            {
                EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchInputPanel);
            }
        });
    }

    private void ButtonSetting(string str = "")
    {
         isMoving = true;
        fileSearchPanel.Show();
        Debug.Log("¤±¤¤¤·¤©");
        int ex = 1;
        switch(str)
        {
            case "infoCheck":
                ex = 0;
                break;
            case "profileSystem":
                ex = 1;
                break;
            case "fileSearch":
                ex = 2;
                break;

            default:
                ex = -1;
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            isOpen = true;
            isMoving = false;
            if (ex == i)
            {
                btnList[i].image.color = blackColor;
                btnTextList[i].color = whiteColor;
            }
            else
            {
                btnList[i].image.color = whiteColor;
                btnTextList[i].color = blackColor;
            }
        }
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
