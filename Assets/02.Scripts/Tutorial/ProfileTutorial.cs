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

    public static EGuideObject guideObjectName;

    [SerializeField]
    private int targetID = 76;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
    }

    private void CreatePopUp(object[] ps)
    {
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

    private IEnumerator StartProfileTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        DataManager.Inst.SetProfilerTutorial(true);
        ProfileChattingSystem.OnChatEnd += FileExploreGuide;

        GetInfoEvent();

        StartChatting(0);
    }

    private void OpenLibrary()
    {

    }

    private void GetInfoEvent()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, GetInfo);
    }

    public void GetInfo(object[] ps)
    {
        int id = (int)ps[1];
        if(id == targetID)
        {
            EventManager.StopListening(EProfileEvent.FindInfoText, GetInfo);
            ProfileChattingSystem.OnChatEnd += TutorialEnd;
            StartChatting(1);
        }
    }

    private void FileExploreGuide()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 2f);
        guideObjectName = EGuideObject.Explore;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.Explore });
    }

    private void IncidentTabGuide()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookIncidentTab, 2f);
        guideObjectName = EGuideObject.IncidentTab;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab });
    }
    private void CharacterTabGuide()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookCharacterTab, 2f);
        guideObjectName = EGuideObject.CharacterTab;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterTab });
    }

    private void TutorialEnd()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        DataManager.Inst.SetProfilerTutorial(false);
    }

    public void StartCompleteProfileTutorial(object[] ps = null)
    {
        ProfileChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfileChattingSystem.OnChatEnd = null;

        GuideUISystem.EndAllGuide?.Invoke();

        StopAllCoroutines();

    }
}
