using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
   
    private Dictionary<string, ImageViewerDataSO> imageVierwerDictionary;

    public ImageViewerDataSO GetImageViewerData(string fileName)
    {
        return imageVierwerDictionary[fileName];
    }

    private async void LoadImageViewerDataAssets(Action callBack)
    {
        imageVierwerDictionary = new Dictionary<string, ImageViewerDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ImageViewerData");
        // ImageViewerData 이 Label을 들고있는 데이터들의 경로를 리스트로 묶어서 불러오는 거

        await handle.Task;

        // 예외처리

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ImageViewerDataSO>(handle.Result[i]).Task;
            await task;

            imageVierwerDictionary.Add(task.Result.fileName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
