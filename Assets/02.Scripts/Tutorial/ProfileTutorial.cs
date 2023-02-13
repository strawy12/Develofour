﻿using System.Collections;
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
        Debug.Log("현재 업데이트에 디버그 코드가 있습니다.");

        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });

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
            NoticeProfileChattingTutorial();
            //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); 끝나고 해야하는거
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
        
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "안녕하십니까?" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "프로파일러를 설치해주셔서 감사합니다." });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "우선 이 프로그램을 사용하는 방법을 알려드리겠습니다" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "먼저 간단하게 정보를 수집해볼까요?" });
        yield return new WaitForSeconds(1.5f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "배경화면을 확인해 주세요." });
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart, new object[0] { });
    }

    public void NoticeProfileChattingTutorial()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "잘하셨습니다." });
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "이렇게 정보를 클릭할 시 정보가 수집이 완료가 되며 이 정보는 왼쪽 패널에 자동으로 정리가 됩니다." });
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { "정리된 정보를 한번 알아보러갈까요?" });
        EventManager.TriggerEvent(ETutorialEvent.ProfileInfoStart, new object[0] { });
    }

    public IEnumerator StartProfileLastTutorial()
    {
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
