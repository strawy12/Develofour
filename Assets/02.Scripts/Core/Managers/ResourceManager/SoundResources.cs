using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private List<AudioAssetSO> audioResourceList;

    public AudioAssetSO GetAudioResource(Sound.EAudioType audioType)
    {
        return audioResourceList.Find(x => x.AudioType == audioType);
    }

    private async void LoadAudioAssets(Action callBack)
    {
        audioResourceList = new List<AudioAssetSO>();

        var handle = Addressables.LoadResourceLocationsAsync("Sound", typeof(AudioAssetSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<AudioAssetSO>(handle.Result[i]).Task;
            await task;

            audioResourceList.Add(task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
