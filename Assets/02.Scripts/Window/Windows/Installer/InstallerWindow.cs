﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI;


public class InstallerWindow : Window
{
    [SerializeField]
    private List<InstallerScreen> screenList;
    [SerializeField]
    private TextButton nextBtn;
    [SerializeField]
    private Button backBtn;
    [SerializeField]
    private Button cancelBtn;


    private int currentIdx = 0;

    public TextButton NextBtn => nextBtn;
    public Button BackBtn => backBtn;
    public Button CancelBtn => cancelBtn;

    protected override void Init()
    {
        base.Init();

        currentIdx = 0;
        screenList.ForEach(x => x.Init(this));

        nextBtn.onClick?.AddListener(NextScreen);
        backBtn.onClick?.AddListener(BackScreen);
        // 팝업창 띄울지 고민
        cancelBtn.onClick?.AddListener(WindowClose);
        
        ChangeScreen(currentIdx);
    }

    public void NextScreen()
    {
        ChangeScreen(currentIdx + 1);
    }

    public void BackScreen()
    {
        ChangeScreen(currentIdx - 1);
    }

    public void ChangeScreen(int idx)
    {
        if (Define.IsBoundOver(idx, screenList.Count)) return;

        screenList[currentIdx].gameObject.SetActive(false);
        
        currentIdx = idx;

        screenList[currentIdx].gameObject.SetActive(true);
        screenList[currentIdx].EnterScreen();
    }

    public void UnInteractableCancelBtn()
    {
        windowBar.CloseBtn.interactable = false;
        cancelBtn.interactable = false;
    }

    public void EndInstall()
    {
        if (DataManager.Inst.SaveData.isProfilerInstall)
            return;
        EventManager.TriggerEvent(EProfilerEvent.InstalledProfiler);

        DataManager.Inst.SaveData.isProfilerInstall = true;
        FileManager.Inst.AddFile(FileManager.Inst.GetAdditionalFile(Constant.FileID.PROFILER), Constant.FileID.BACKGROUND);
    }

    public void CheckOpenWindow(bool isWindowOpen)
    {
        if (isWindowOpen)
        {
            WindowManager.Inst.WindowOpen(EWindowType.ProfilerWindow, FileManager.Inst.GetAdditionalFile(Constant.FileID.PROFILER));
        }
    }
}
