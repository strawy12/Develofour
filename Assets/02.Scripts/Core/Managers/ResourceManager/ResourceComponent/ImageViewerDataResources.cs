using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ImageViewerDataResources : ResourcesComponent
{
    //public ImageViewerDataSO GetImageViewerData(string fileId)
    //{
    //    return imageVierwerDictionary[fileId];
    //}

    private async void LoadImageViewerDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ImageViewerData");
        // ImageViewerData 이 Label을 들고있는 데이터들의 경로를 리스트로 묶어서 불러오는 거

        await handle.Task;

        // 예외처리

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ImageViewerDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
