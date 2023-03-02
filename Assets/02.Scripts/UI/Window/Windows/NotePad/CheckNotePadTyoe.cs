﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNotePadTyoe : MonoBehaviour
{
    private FileSO currentFile;

    private void Awake()
    {

    }
    public void Setting(FileSO file)
    {
        currentFile = file;
        CheckFileOwnerInfo();
    }

    private void CheckFileOwnerInfo()
    {
        if (currentFile == null) return;
        if (currentFile.windowName == "의뢰자 정보")
        {
            EventManager.TriggerEvent(EDecisionEvent.ClickOwnerNameText);

            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { EProfileCategory.Owner, "OwnerName" });
        }

    }
}
