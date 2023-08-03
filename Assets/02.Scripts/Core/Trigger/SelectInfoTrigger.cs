using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SelectInfoTrigger : InformationTrigger, IPointerClickHandler
{
    [SerializeField]
    private string wrongMonologID;

    [SerializeField]
    private GameObject[] answerObj;

    [SerializeField]
    private GameObject wholeObj;

    public bool isClear;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj == wholeObj)
        {
            MonologSystem.OnStartMonolog?.Invoke(wrongMonologID, false);
            return;
        }

        foreach(var answer in answerObj)
        {
            if(obj == answer)
            {
                ClickAnswer();
                return;
            }
        }
    }

    private void ClickAnswer()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall || !DataManager.Inst.IsStartProfilerTutorial()) return;

        GetInfo();

        //정답 독백 끝나고 
        isClear = true;

    }

}
