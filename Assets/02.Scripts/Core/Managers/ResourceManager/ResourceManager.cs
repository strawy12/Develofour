using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private IEnumerator Start()
    {
        int cnt = 7;

        LoadAudioAssets(() => cnt--);
        LoadNoticeDatas(() => cnt--);
        LoadTextDataSOAssets(() => cnt--);
        LoadImageViewerDataAssets(() => cnt--);
        LoadNotepadDataAssets(() => cnt--);
        LoadMediaPlayerDataAssets(() => cnt--);
        LoadProfileCategoryDataResourcesAssets(() => cnt--);
        yield return new WaitUntil(() => cnt == 0);

        EventManager.TriggerEvent(ECoreEvent.EndLoadResources);
    }
}
