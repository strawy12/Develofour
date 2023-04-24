using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[System.Serializable]
public class DebugEvent
{
    public KeyCode keyCode;
    public UnityEvent debugEvent;
}

public class Debugger : MonoBehaviour
{
#if UNITY_EDITOR
    private int testTextCnt = 0;

    [SerializeField]
    private List<DebugEvent> debugEventList;

    [SerializeField]
    private FileSO todoFile;

    [SerializeField]
    private StartCutScene cutScene;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (GameManager.Inst.GameState == EGameState.PlayTitle)
                return;

            if(cutScene != null)
            {
                if(!cutScene.isSkip)
                {
                    cutScene.isSkip = true;
                    cutScene.DOKill(false);
                    if(cutScene.isScreamSound)
                        Sound.OnImmediatelyStop(Sound.EAudioType.StartCutSceneScream);
                    cutScene.StartLoading();
                }
            }
 
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MonologSystem.OnStopMonolog?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            //디버그용 스킵 코드 이벤트까지 지워주기
            if(GameManager.Inst.GameState == EGameState.Tutorial)
                EventManager.TriggerEvent(EDebugSkipEvent.TutorialSkip);
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }


        if(Input.GetKeyDown(KeyCode.A))
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { EProfileCategory.InvisibleInformation, "BranchID" ,null});
        }
    }

#endif

}
