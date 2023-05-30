using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Transform poolParent;
    public void Start()
    {
        StartCoroutine(StartGetData());
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();
    }
    private IEnumerator StartGetData()
    {
        int cnt = 18;

        LoadAudioAssets(() => cnt--);
        LoadNoticeDatas(() => cnt--);
        LoadAIChattingTextDataSOAssets(() => cnt--);
        LoadMonologTextDataAssets(() => cnt--);
        LoadImageViewerDataAssets(() => cnt--);

        LoadNotepadDataAssets(() => cnt--);
        LoadMediaPlayerDataAssets(() => cnt--);
        LoadProfileCategoryDataResourcesAssets(() => cnt--);
        LoadCharacterDataDataSOAssets(() => cnt--);
        LoadBrowserShortcutDataResourcesAssets(() => cnt--);

        LoadHarmonyShortcutDataResourcesAssets(() => cnt--);
        LoadMailDataAssets(() => cnt--);
        LoadVideoPlayercDataAssets(() => cnt--);
        LoadRequestCallDataAssets(() => cnt--);
        LoadIncomingCallDataAssets(() => cnt--);

        LoadProfileInfoDataAssets(() => cnt--);
        LoadLockImage(() => cnt--);
        LoadBackgroundBGMWindowDataResourcesAssets(() => cnt--);
        yield return new WaitUntil(() => cnt <= 0);

        GameManager.Inst.Init();
    }

}
