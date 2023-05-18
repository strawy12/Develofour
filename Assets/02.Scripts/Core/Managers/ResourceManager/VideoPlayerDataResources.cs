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
        // ImageViewerData 이 Label을 들고있는 데이터들의 경로를 리스트로 묶어서 불러오는 거

        await handle.Task;

        // 예외처리

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
