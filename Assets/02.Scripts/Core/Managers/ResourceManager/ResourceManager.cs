using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private IEnumerator Start()
    {
        int cnt = 6;

        LoadAudioAssets(() => cnt--);
        LoadNoticeDatas(() => cnt--);
        LoadTextDataSOAssets(() => cnt--);
        LoadImageViewerDataAssets(() => cnt--);
        LoadNotepadDataAssets(() => cnt--);
        LoadMediaPlayerDataAssets(() => cnt--);

        yield return new WaitUntil(() => cnt == 0);

        EventManager.TriggerEvent(ECoreEvent.EndLoadResources);
    }
}
