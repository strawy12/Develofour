using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private IEnumerator Start()
    {
        int cnt = 9;

        LoadAudioAssets(() => cnt--);
        LoadNoticeDatas(() => cnt--);
        LoadAIChattingTextDataSOAssets(() => cnt--);
        LoadMonologTextDataAssets(() => cnt--);
        LoadImageViewerDataAssets(() => cnt--);
        LoadNotepadDataAssets(() => cnt--);
        LoadMediaPlayerDataAssets(() => cnt--);
        LoadProfileCategoryDataResourcesAssets(() => cnt--);
        LoadCharacterDataDataSOAssets(() => cnt--);
        yield return new WaitUntil(() => cnt <= 0);

        EventManager.TriggerEvent(ECoreEvent.EndLoadResources);

        GameManager.Inst.OnStartCallback?.Invoke();
    }
}
 