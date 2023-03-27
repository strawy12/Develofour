using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSearchTutorial : MonoBehaviour
{
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
            Debug.Log("EndEvent");
            WindowManager.Inst.PopupOpen(profiler);
        };
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartSearchTutoMonolog, 0.3f);
    }

    private void TutorialStart(object[] ps)
    {
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfileChattingSystem.OnChatEnd += delegate { EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchButton); };
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { EAIChattingTextDataType.SearchTutoClickBtn });
    }

    private void OnClickGuideSearchButton(object[] ps)
    {
        GuideUISystem.EndGuide?.Invoke();
        ProfileChattingSystem.OnChatEnd += delegate { EventManager.TriggerEvent(EProfileSearchTutorialEvent.GuideSearchInputPanel); };
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { EAIChattingTextDataType.SearchTutoClickInput });
    }

    private void SearchName(object[] ps)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { EAIChattingTextDataType.CompleteSearchTuto });

        EndTutorial(ps);
    }

    private void EndTutorial(object[] ps)
    {
        DataManager.Inst.SaveData.isSearchFileTuto = true;
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }
}
