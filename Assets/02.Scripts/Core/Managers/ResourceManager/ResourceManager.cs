using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Transform poolParent;

    private Dictionary<Type, ResourcesComponent> resourceComponets;

    public void Start()
    {
        StartCoroutine(StartGetData());
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();
    }
    private IEnumerator StartGetData()
    {
        var list = GetComponentsInChildren<ResourcesComponent>();
        int cnt = list.Length + 3;

        LoadNoticeDatas(()=> cnt--);
        LoadLockImage(() => cnt--);
        LoadAudioAssets(() => cnt--);

        foreach (var resource in list)
        {
            resource.LoadResourceDataAssets(() => cnt--);
            resourceComponets.Add(resource.soType, resource);
        }
        yield return new WaitUntil(() => cnt == 0);
        // ���� ����
        // �̰Ŵ� ���� ResourcesComponent �� �ڽ� ������Ʈ(������Ʈ) �ϳ��� �� Addressable Label ����
        // �װ� ���ҽ� �Ŵ����� ����Ʈ�� ����ֵ�, ��������Ʈ �� ���ϵ� �̷��� �Ἥ ���ε�����
        // �ڽ� ������Ʈ�� base�� ��ӹ޾� �����, ���� �Լ��� ��������ش�.
        GameManager.Inst.Init();
    }

    public T GetResource<T>(string key) where T : ResourceSO
    {
        if (!resourceComponets.ContainsKey(typeof(T)))
        {
            Debug.Log("�ش� �̸��� ResourceComponent�� �����ϴ�.");
            return null;
        }
        return resourceComponets[typeof(T)].GetResource(key) as T;
    }

    public List<T> GetResourceList<T>() where T : ResourceSO
    {
        if (!resourceComponets.ContainsKey(typeof(T)))
        {
            Debug.Log("�ش� �̸��� ResourceComponent�� �����ϴ�.");
            return null;
        }
        return resourceComponets[typeof(T)].GetRsourceDictionary().Select(x=>x.Value as T).ToList();
    }


}
