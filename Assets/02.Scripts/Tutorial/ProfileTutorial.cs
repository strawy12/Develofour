using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileTutorial : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(StartProfileOpenTutorial());
        }

    }

    public GameObject tutorialPanel;

    void Start()
    {
        Debug.Log("���� ������Ʈ�� ����� �ڵ尡 �ֽ��ϴ�.");
        EventManager.StartListening(ETutorialEvent.BackgroundSelect, delegate
        {
            EventManager.TriggerEvent(ETutorialEvent.BackgroundSignEnd, new object[0]);
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); ���߿�
            //���� usb ������ �ƴ϶�� ��������. �װ� �ɷ��ִ°� ���� �ڵ�(���� ���� ����)
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoStart, new object[0]);
        });

        EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoSelect, delegate
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoEnd, new object[0]);
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); ������ �ؾ��ϴ°�
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
