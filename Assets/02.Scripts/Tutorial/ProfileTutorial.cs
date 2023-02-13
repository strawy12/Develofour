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
        Debug.Log("현재 업데이트에 디버그 코드가 있습니다.");
        EventManager.StartListening(ETutorialEvent.BackgroundSelect, delegate
        {
            EventManager.TriggerEvent(ETutorialEvent.BackgroundSignEnd, new object[0]);
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); 나중에
            //만약 usb 폴더가 아니라면 오류터짐. 그걸 걸러주는게 위에 코드(아직 제작 안함)
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoStart, new object[0]);
        });

        EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoSelect, delegate
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoEnd, new object[0]);
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); 끝나고 해야하는거
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
