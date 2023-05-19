using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfileTutorial : MonoBehaviour
{
    [SerializeField]
    private TutorialTextSO profileTutorialTextData;
    [SerializeField]
    private float findNameGuideDelay = 60f;
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;
    [SerializeField]
    private RectTransform USBRect;
    [SerializeField]
    private RectTransform reportRect;
    [SerializeField]
    private Library library;

    [SerializeField]
    private int targetID = 76;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
        //EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });
        //EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, StartCompleteProfileTutorial);

        // 만약 USB 화면 들어가면
    }

    private void CreatePopUp(object[] ps)
    {
        Debug.Log("OpenPop");
#if UNITY_EDITOR
        WindowManager.Inst.PopupOpen(profiler, profileTutorialTextData.popText, () => StartTutorial(null), () => StartCompleteProfileTutorial());
#else
        WindowManager.Inst.PopupOpen(profiler, profileTutorialTextData.popText, () => StartTutorial(null), null);
#endif
    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfileTutorial());
    }

    public void StartChatting(int textListIndex)
    {
        ProfileChattingSystem.OnPlayChatList?.Invoke(profileTutorialTextData.tutorialTexts[textListIndex].data, 1.5f, true);
    }

    private void LibraryClose()
    {
        Debug.Log("으악 챗 엔드");
        if(WindowManager.Inst.IsExistWindow(EWindowType.Directory))
        {
            Library library = WindowManager.Inst.GetWindow(EWindowType.Directory, 11) as Library;
            library.WindowClose();
        }
        
    }

    private IEnumerator StartProfileTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        DataManager.Inst.SetProfilerTutorial(true);
        ProfileChattingSystem.OnChatEnd += StartDelay;

        GetVictimNameEvent();

        StartChatting(0);
    }

    private void GetVictimNameEvent()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, GetVictimName);
    }

    public void GetVictimName(object[] ps)
    {
        int id = (int)ps[1];
        Debug.Log(id);
        if(id == targetID)
        {
            EventManager.StopListening(EProfileEvent.FindInfoText, GetVictimName);
            ProfileChattingSystem.OnChatEnd += TutorialEnd;
            StartChatting(1);
        }
    }

    private void StartDelay()
    {
        //LibraryClose();
        Debug.Log("왜안됨");
        GuideUISystem.OnGuide(libraryRect);

        //StartCoroutine(FindNameCoroutine());
    }

    private void TutorialEnd()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        DataManager.Inst.SetProfilerTutorial(false);
    }

    //public IEnumerator FindNameCoroutine()
    //{
    //    yield return new WaitForSeconds(findNameGuideDelay);
    //    SearchGuideStart();
    //}

    //private void SearchGuideStart()
    //{
    //    ProfileChattingSystem.OnChatEnd += () =>
    //    {
    //        Debug.Log("으악!");
    //        EventManager.TriggerEvent(ETutorialEvent.SearchBtnGuide);
    //        EventManager.StartListening(ETutorialEvent.ClickSearchBtn, StartSearchName);
    //    };
    //    StartChatting(1);
    //}

    //private void StartSearchName(object[] ps)
    //{
    //    EventManager.StopListening(ETutorialEvent.ClickSearchBtn, StartSearchName);

    //    GuideUISystem.EndAllGuide?.Invoke();

    //    ProfileChattingSystem.OnChatEnd += () =>  EventManager.StartListening(ETutorialEvent.SearchNameText, SearchName);
    //    StartChatting(2);
    //}

    //private void SearchName(object[] ps)
    //{
    //    Debug.Log("1");
    //    EventManager.StopListening(ETutorialEvent.SearchNameText, SearchName);
    //    ProfileChattingSystem.OnChatEnd += () => StartCompleteProfileTutorial();
    //    StartChatting(3);
    //}

    public void StartCompleteProfileTutorial(object[] ps = null)
    {
        ProfileChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfileChattingSystem.OnChatEnd = null;

        GuideUISystem.EndAllGuide?.Invoke();

        StopAllCoroutines();

        EventManager.StopListening(ETutorialEvent.EndClickInfoTutorial, StartCompleteProfileTutorial);

        //ProfileChattingSystem.OnChatEnd += StartProfileEnd;
        //StartChatting(4);
    }

    //public void StartProfileEnd()
    //{
    //    DataManager.Inst.SetIsClearTutorial(ETutorialType.Profiler, true);
    //    EventManager.StopListening(ETutorialEvent.TutorialStart, StartTutorial);

    //    GameManager.Inst.ChangeGameState(EGameState.Game);

    //    MonologSystem.OnEndMonologEvent = () => EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[] { "01012345678" });
    //    CallSystem.Inst.OnAnswerCall(ECharacterDataType.Assistant, Constant.MonologKey.ENDPROFILETUTORIALCHATLOG);
    //}
}
