using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileTutorial : MonoBehaviour
{
    public string[] startAIChatting;
    public string[] findNoticeAIChatting;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
    }

    public IEnumerator StartProfileTutorial()
    {
        WindowManager.Inst.StartTutorialSetting();
        //tutorialPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        EventManager.StartListening(ETutorialEvent.ProfileInfoEnd, delegate { StartCoroutine(StartProfileLastTutorial()); });

        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);

        for (int i = 0; i < startAIChatting.Length; i++)
        {
            if (i == 3)
            {
                MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog1, 0.1f, 1);
            }
            AIChatting(startAIChatting[i]);

            yield return new WaitForSeconds(1.5f);
        }
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 0.1f);

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { NoticeProfileChattingTutorial(); });

    }

    private void AIChatting(string str)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
    }

    public IEnumerator NoticeProfileChattingTutorial()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        foreach (string str in findNoticeAIChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(1.5f);
        }
        MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog2, 0.1f, 3);

        EventManager.TriggerEvent(ETutorialEvent.ProfileInfoStart);
    }

    public IEnumerator StartProfileLastTutorial()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "이렇게 보시다싶이 저희가 수집한 정보인 이름만 적혀져있는 것을 볼 수 있습니다" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "이제 앞으로 정보에 비어져있는 부분을 찾아서 클릭하시면 정보를 수집할 수 있습니다" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "앞으로 잘 부탁드립니다" });
        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });

        GameManager.Inst.ChangeGameState(EGameState.Game);
    }
}
