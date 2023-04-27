using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<EMailType, MailDataSO> mailDataSOList;
    public Dictionary<EMailType, MailDataSO> MailDataSOList => mailDataSOList;

    public MailDataSO GetMailTextData(EMailType type)
    {
        return mailDataSOList[type];
    }

    private async void LoadMailDataAssets(Action callBack)
    {
        mailDataSOList = new Dictionary<EMailType, MailDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("MailData", typeof(MailDataSO));
        await handle.Task;
        Debug.Log(handle.Result.Count);
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<MailDataSO>(handle.Result[i]).Task;
            await task;

            mailDataSOList.Add(task.Result.Type, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
