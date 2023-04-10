using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideButtonTutorial : MonoBehaviour
{

    private enum EGuideButtonTutorialChatting
    {
        Start = 0,
        ClickMoveBtn,
        ClickAnyBtn,
    }
    [SerializeField]
    private TutorialTextSO tutorialTextList;

    [SerializeField]
    private FileSO profiler;

    private void Start()
    {
        EventManager.StartListening(EGuideButtonTutorialEvent.TutorialStart, StartPopupOpen);
    }

    private void StartPopupOpen(object[] ps)
    {
        EventManager.StopListening(EGuideButtonTutorialEvent.TutorialStart, StartPopupOpen);

        Debug.Log("openPopup");
        WindowManager.Inst.PopupOpen(profiler, tutorialTextList.popText, delegate { TutorialStart(null); }, delegate { EndTutorial(); });
    }

    private void TutorialStart(object[] ps)
    {
        WindowManager.Inst.WindowOpen(EWindowType.ProfileWindow, profiler);

        DataManager.Inst.SetIsStartTutorial(ETutorialType.Guide, true);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);

        ProfileChattingSystem.OnChatEnd += delegate
        {
            EventManager.StartListening(EGuideButtonTutorialEvent.ClickMoveBtn, ClickMoveBtn);
            EventManager.TriggerEvent(EGuideButtonTutorialEvent.GuideMoveBtn);
        };
        ProfileChattingSystem.OnPlayChatList?.Invoke(tutorialTextList.tutorialTexts[(int)EGuideButtonTutorialChatting.Start].data, 1.5f, true);

    }

    private void ClickMoveBtn(object[] ps)
    {
        GuideUISystem.EndAllGuide?.Invoke();

        EventManager.StopListening(EGuideButtonTutorialEvent.ClickMoveBtn, ClickMoveBtn);

        EventManager.StartListening(EGuideButtonTutorialEvent.ClickAnyBtn, ClickAnyBtn);

        ProfileChattingSystem.OnPlayChatList?.Invoke(tutorialTextList.tutorialTexts[(int)EGuideButtonTutorialChatting.ClickMoveBtn].data, 1.5f, true);
    }

    private void ClickAnyBtn(object[] ps)
    {
        EventManager.StopListening(EGuideButtonTutorialEvent.ClickAnyBtn, ClickAnyBtn);

        ProfileChattingSystem.OnChatEnd += delegate
        {
            EndTutorial();
        };
        ProfileChattingSystem.OnPlayChatList?.Invoke(tutorialTextList.tutorialTexts[(int)EGuideButtonTutorialChatting.ClickAnyBtn].data, 1.5f, true);
    }

    private void EndTutorial()
    {
        DataManager.Inst.SetIsStartTutorial(ETutorialType.Guide, true);
        DataManager.Inst.SetIsClearTutorial(ETutorialType.Guide, true);

        GameManager.Inst.ChangeGameState(EGameState.Game);
    }

}
