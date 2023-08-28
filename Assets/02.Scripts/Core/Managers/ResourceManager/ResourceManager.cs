using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Transform poolParent;

    public Action OnCompleted;
    public int cnt {get; private set; }
    public void Start()
    {
        cnt = 20;
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();

        StartCoroutine(StartGetData());
    }
    private IEnumerator StartGetData()
    {
        LoadAudioAssets(LoadComplete);
        LoadNoticeDatas(LoadComplete);
        LoadAIChattingTextDataSOAssets(LoadComplete);
        LoadMonologTextDataAssets(LoadComplete);
        LoadImageViewerDataAssets(LoadComplete);

        LoadNotepadDataAssets(LoadComplete);
        LoadMediaPlayerDataAssets(LoadComplete);
        LoadProfilerCategoryResourcesAssets(LoadComplete);
        LoadCharacterDataDataSOAssets(LoadComplete);
        LoadBrowserShortcutDataResourcesAssets(LoadComplete);

        LoadHarmonyShortcutDataResourcesAssets(LoadComplete);
        LoadMailDataAssets(LoadComplete);
        LoadVideoPlayercDataAssets(LoadComplete);
        LoadRequestCallDataAssets(LoadComplete);
        LoadIncomingCallDataAssets(LoadComplete);

        LoadProfilerInfoDataAssets(LoadComplete);
        LoadLockImage(LoadComplete);
        LoadBackgroundBGMWindowDataResourcesAssets(LoadComplete);
        LoadFileLockDataAssets(LoadComplete);
        LoadTriggerDataSOResourcesAssets(LoadComplete);

        yield return new WaitUntil(() => cnt <= 0);
        yield return new WaitForSeconds(2f);

        DataManager.Inst.D_CheckDirectory();

        GameManager.Inst.Init();
    }

    public void LoadComplete()
    {
        cnt--;
        OnCompleted?.Invoke();
    }

}
