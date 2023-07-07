using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Transform poolParent;
    public void Start()
    {
        StartCoroutine(StartGetData());
        DataLoadingScreen.OnShowLoadingScreen?.Invoke();
    }
    private IEnumerator StartGetData()
    {
        yield return null;
        // 로직 변경
        // 이거는 차라리 ResourcesComponent 의 자식 컴포넌트(오브젝트) 하나당 한 Addressable Label 관리
        // 그걸 리소스 매니저가 리스트로 들고있든, 겟컴포넌트 인 차일드 이런거 써서 전부들고오고
        // 자식 컴포넌트를 base를 상속받아 만들고, 공통 함수를 실행시켜준다.

        GameManager.Inst.Init();
    }

}
