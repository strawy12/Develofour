using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    public void Start()
    {
        StartCoroutine(StartGetData());
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();
    }
    private IEnumerator StartGetData()
    {
        int cnt = 11;

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
        yield return new WaitUntil(() => cnt <= 0);

        EventManager.TriggerEvent(ECoreEvent.EndLoadResources);

        GameManager.Inst.OnStartCallback?.Invoke();
    }
}
 