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
        GuideUISystem.OnGuide(libraryRect);
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

        EventManager.StopListening(ETutorialEvent.EndClickInfoTutorial, StartCompleteProfileTutorial);
    }
}
