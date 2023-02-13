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
        
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�ȳ��Ͻʴϱ�?" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�������Ϸ��� ��ġ���ּż� �����մϴ�." });
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�켱 �� ���α׷��� ����ϴ� ����� �˷��帮�ڽ��ϴ�" });
        yield return new WaitForSeconds(2f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "���� �����ϰ� ������ �����غ����?" });

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart, new object[0] { });

    }


}
