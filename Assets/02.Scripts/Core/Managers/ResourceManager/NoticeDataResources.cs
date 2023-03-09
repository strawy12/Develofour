using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<ENoticeType, NoticeDataSO> noticeDataList;

    public NoticeDataSO GetNoticeData(ENoticeType noticeType)
    {
        return noticeDataList[noticeType];
    }

    private async void LoadNoticeDatas(Action callBack)
    {
        noticeDataList = new Dictionary<ENoticeType, NoticeDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("NoticeData", typeof(NoticeDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<NoticeDataSO>(handle.Result[i]).Task;
            await task;

            noticeDataList.Add(task.Result.NoticeDataType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
