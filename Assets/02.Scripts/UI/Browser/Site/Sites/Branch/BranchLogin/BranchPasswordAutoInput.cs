using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BranchPasswordAutoInput : AutoAnswerInputFiled
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if(DataManager.Inst.SaveData.branchPassword.Length <= 3)
        {
            return;
        }

        inputSystem.ShowPanel(inputField, DataManager.Inst.SaveData.branchPassword);
    }
}
