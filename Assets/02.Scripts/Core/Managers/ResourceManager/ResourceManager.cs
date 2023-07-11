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
        // 로직 변경
        // 이거는 차라리 ResourcesComponent 의 자식 컴포넌트(오브젝트) 하나당 한 Addressable Label 관리
        // 그걸 리소스 매니저가 리스트로 들고있든, 겟컴포넌트 인 차일드 이런거 써서 전부들고오고
        // 자식 컴포넌트를 base를 상속받아 만들고, 공통 함수를 실행시켜준다.
        GameManager.Inst.Init();
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
