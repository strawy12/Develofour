using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileTutorial : MonoBehaviour
{
    private void Awake()
    {
        EventManager.StartListening(ETutorialEvent.StartTutorial, StartTutorial);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("tutorial debug");
            StartCoroutine(StartProfileOpenTutorial());
        }

    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfileOpenTutorial());
        EventManager.StopListening(ETutorialEvent.StartTutorial, StartTutorial);

    }


    public GameObject tutorialPanel;

    void Start()
    {
        

        EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoSelect, delegate
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoEnd, new object[0]);
        });

      

    }


    public IEnumerator StartProfileOpenTutorial()
    {
        //tutorialPanel.SetActive(true);

        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "안녕하십니까?" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "프로파일러를 설치해주셔서 감사합니다." });
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "우선 이 프로그램을 사용하는 방법을 알려드리겠습니다" });
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "먼저 간단하게 정보를 수집해볼까요?" });

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart, new object[0] { });

    }


}
