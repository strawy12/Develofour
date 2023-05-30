using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, MediaPlayerDataSO> mediaPlayerDataList;

    public MediaPlayerDataSO GetMediaPlayerData(int key)
    {
        if(mediaPlayerDataList.ContainsKey(key))
            return mediaPlayerDataList[key];
        return null;
    }

    private async void LoadMediaPlayerDataAssets(Action callBack)
    {
        mediaPlayerDataList = new Dictionary<int, MediaPlayerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("MediaPlayerData", typeof(MediaPlayerDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<MediaPlayerDataSO>(handle.Result[i]).Task;
            await task;

            mediaPlayerDataList.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
