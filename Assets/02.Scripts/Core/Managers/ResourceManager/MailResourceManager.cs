using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<int, MailDataSO> mailDataSOList;
    public Dictionary<int, MailDataSO> MailDataSOList => mailDataSOList;

    public MailDataSO GetMailTextData(int type)
    {
        return mailDataSOList[type];
    }

    private async void LoadMailDataAssets(Action callBack)
    {
        mailDataSOList = new Dictionary<int, MailDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MailData", typeof(MailDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<MailDataSO>(handle.Result[i]).Task;
            await task;

            mailDataSOList.Add(task.Result.mailID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
