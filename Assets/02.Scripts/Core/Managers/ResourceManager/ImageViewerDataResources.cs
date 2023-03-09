using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, ImageViewerDataSO> imageVierwerList;

    public ImageViewerDataSO GetImageViewerData(string fileName)
    {
        return imageVierwerList[fileName];
    }

    private async void LoadImageViewerDataAssets(Action callBack)
    {
        imageVierwerList = new Dictionary<string, ImageViewerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ImageViewerData", typeof(ImageViewerDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ImageViewerDataSO>(handle.Result[i]).Task;
            await task;

            imageVierwerList.Add(task.Result.fileName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
