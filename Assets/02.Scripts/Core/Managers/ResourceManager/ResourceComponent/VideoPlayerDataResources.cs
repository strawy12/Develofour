using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class VideoPlayerDataResources : ResourcesComponent
{

    public override async void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("VideoPlayerData");
        // ImageViewerData �� Label�� ����ִ� �����͵��� ��θ� ����Ʈ�� ��� �ҷ����� ��

        await handle.Task;

        // ����ó��

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<VideoPlayerDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.FileID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }

}
