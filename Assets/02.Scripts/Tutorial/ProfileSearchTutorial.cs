using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSearchTutorial : MonoBehaviour
{
    private enum ESearchTutoChatting
    {
        SearchTutoClickBtn = 0,
        SearchTutoClickInput = 1,
        CompleteSearchTuto,
    }
    [SerializeField]
    public TutorialTextSO textDataList;
    
    [SerializeField]
    private FileSO profiler;
    private void Start()
    {
        EventManager.StartListening(EProfileSearchTutorialEvent.TutorialMonologStart, TutorialStartMonolog);
        EventManager.StartListening(EProfileSearchTutorialEvent.TutorialStart, TutorialStart);
        EventManager.StartListening(EProfileSearchTutorialEvent.ClickSearchButton, OnClickGuideSearchButton);
        EventManager.StartListening(EProfileSearchTutorialEvent.SearchNameText, SearchName);
        EventManager.StartListening(EProfileSearchTutorialEvent.EndTutorial, EndTutorial);

    }

    private void TutorialStartMonolog(object[] ps)
    {
        MonologSystem.OnEndMonologEvent += delegate
        {
            WindowManager.Inst.PopupOpen(profiler, textDataList.popText, delegate { TutorialStart(null); },delegate { EndTutorial(null); });
        };
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartSearchTutoMonolog, 0.3f, true);
    }

    private void TutorialStart(object[] ps)
    {
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfileChattingSystem.OnChatEnd += delegate {
            DataManager.Inst.SetIsStartTutorial(ETutorialType.Search, true);
            EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchButton);
        };

        ProfileChattingSystem.OnPlayChat?.Invoke(textDataList.tutorialTexts[(int)ESearchTutoChatting.SearchTutoClickBtn].data[0], true);
    }

    private void OnClickGuideSearchButton(object[] ps)
    {
        EventManager.StopListening(EProfileSearchTutorialEvent.ClickSearchButton, OnClickGuideSearchButton);
        GuideUISystem.EndGuide?.Invoke();
        ProfileChattingSystem.OnChatEnd += delegate { EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchInputPanel); };

        ProfileChattingSystem.OnPlayChatList?.Invoke(textDataList.tutorialTexts[(int)ESearchTutoChatting.SearchTutoClickInput].data, 1f ,true);
    }

    private void SearchName(object[] ps)
    {
        GuideUISystem.EndGuide?.Invoke();
        ProfileChattingSystem.OnPlayChatList?.Invoke(textDataList.tutorialTexts[(int)ESearchTutoChatting.CompleteSearchTuto].data, 1f, true);

        EndTutorial(ps);
    }

    private void EndTutorial(object[] ps)
    {
        DataManager.Inst.SetIsStartTutorial(ETutorialType.Search, true);   //튜토리얼 스킵했을 때 상황을 위한 저장
        DataManager.Inst.SetIsClearTutorial(ETutorialType.Search, true);

        DataManager.Inst.SaveData.isSearchFileTuto = true;
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }
}
