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
            EventManager.TriggerEvent(ETutorialEvent.TutorialStart, new object[0]);
        }
    }

    public GameObject tutorialPanel;

    void Start()
    {
        Debug.Log("���� ������Ʈ�� ����� �ڵ尡 �ֽ��ϴ�.");

        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });

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
            NoticeProfileChattingTutorial();
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); ������ �ؾ��ϴ°�
        });

        EventManager.StartListening(ETutorialEvent.ProfileInfoSelect, delegate 
        {
            EventManager.TriggerEvent(ETutorialEvent.ProfileInfoEnd, new object[0]);
            StartCoroutine(StartProfileLastTutorial());
        });
    }


    public IEnumerator StartProfileTutorial()
    {
        //tutorialPanel.SetActive(true);

        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�ȳ��Ͻʴϱ�?" });
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�������Ϸ��� ��ġ���ּż� �����մϴ�." });
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�켱 �� ���α׷��� ����ϴ� ����� �˷��帮�ڽ��ϴ�" });
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "���� �����ϰ� ������ �����غ����?" });
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "���ȭ���� Ȯ���� �ּ���." });
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart, new object[0] { });
    }

    public void NoticeProfileChattingTutorial()
    {
        //ai ä�� �˸� ����
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "���ϼ̽��ϴ�." });
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�̷��� ������ Ŭ���� �� ������ ������ �Ϸᰡ �Ǹ� �� ������ ���� �гο� �ڵ����� ������ �˴ϴ�." });
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "������ ������ �ѹ� �˾ƺ��������?" });
        EventManager.TriggerEvent(ETutorialEvent.ProfileInfoStart, new object[0] { });
    }

    public IEnumerator StartProfileLastTutorial()
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "�̷��� ���ôٽ��� ���� ������ ������ �̸��� �������ִ� ���� �� �� �ֽ��ϴ�" });
        yield return new WaitForSeconds(1);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "���� ������ ������ ������ִ� �κ��� ã�Ƽ� Ŭ���Ͻø� ������ ������ �� �ֽ��ϴ�" });
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "������ �� ��Ź�帳�ϴ�" });
        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
        GameManager.Inst.ChangeGameState(EGameState.Game);
    }
}
