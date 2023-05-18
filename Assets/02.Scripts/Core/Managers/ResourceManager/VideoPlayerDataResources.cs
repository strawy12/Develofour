using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{

    private Dictionary<int, VideoPlayerDataSO> videoPlayerDictionary;

    public VideoPlayerDataSO GetVideoPlayerData(int key)
    {
        return videoPlayerDictionary[key];
    }

    private async void LoadVideoPlayercDataAssets(Action callBack)
    {
        videoPlayerDictionary = new Dictionary<int, VideoPlayerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("VideoPlayerData");
        // ImageViewerData �� Label�� ����ִ� �����͵��� ��θ� ����Ʈ�� ��� �ҷ����� ��

        await handle.Task;

        // ����ó��

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<VideoPlayerDataSO>(handle.Result[i]).Task;
            await task;

            videoPlayerDictionary.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
