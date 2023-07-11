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
        // ImageViewerData �� Label�� ����ִ� �����͵��� ��θ� ����Ʈ�� ��� �ҷ����� ��

        await handle.Task;

        // ����ó��

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
