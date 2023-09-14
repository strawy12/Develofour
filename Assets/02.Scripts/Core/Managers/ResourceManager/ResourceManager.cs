using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Transform poolParent;

    private Dictionary<Type, ResourcesComponent> resourceComponets;

    public Action OnCompleted;

    public int maxCnt = 0;

    public void Start()
    {
        resourceComponets = new Dictionary<Type, ResourcesComponent>();
        DataManager.Inst.D_CheckDirectory();
        StartCoroutine(StartGetData());
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();
    }

    private IEnumerator StartGetData()
    {
        var list = GetComponentsInChildren<ResourcesComponent>();
        maxCnt = list.Length + 4;
        LoadNoticeDatas(() => LoadComplete());
        LoadLockImage(() => LoadComplete());
        LoadAudioAssets(() => LoadComplete());
        LoadCharacterprefabs(() => LoadComplete());

        foreach (var resource in list)
        {
            resource.LoadResources(() => LoadComplete());
        }
        yield return new WaitUntil(() => maxCnt <= 0);

        GameManager.Inst.Init();
        yield return new WaitForSeconds(1.5f);
    }

    private void LoadComplete()
    {
        maxCnt--;
        OnCompleted?.Invoke();
    }

    public void AddResourcesComponent(Type resourceType, ResourcesComponent component)
    {
        if (resourceComponets.ContainsKey(resourceType)) return;
        resourceComponets.Add(resourceType, component);
    }

    public T GetResource<T>(string key) where T : ResourceSO
    {
        if (!resourceComponets.ContainsKey(typeof(T)))
        {
            Debug.Log("해당 이름의 ResourceComponent가 없습니다.");
            return null;
        }
        return resourceComponets[typeof(T)].GetResource(key) as T;
    }

    public F GetResourcesComponent<T, F>() where T : ResourceSO where F : ResourcesComponent
    {
        if (!resourceComponets.ContainsKey(typeof(T)))
        {
            Debug.Log("해당 이름의 ResourceComponent가 없습니다.");
            return null;
        }
        return resourceComponets[typeof(T)] as F;
    }


    public List<T> GetResourceList<T>() where T : ResourceSO
    {
        if (!resourceComponets.ContainsKey(typeof(T)))
        {
            Debug.Log("해당 이름의 ResourceComponent가 없습니다.");
            return null;
        }
        return resourceComponets[typeof(T)].GetRsourceDictionary().Select(x=>x.Value as T).ToList();
    }


}
