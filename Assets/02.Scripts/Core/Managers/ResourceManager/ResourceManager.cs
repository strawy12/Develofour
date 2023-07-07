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
        // ���� ����
        // �̰Ŵ� ���� ResourcesComponent �� �ڽ� ������Ʈ(������Ʈ) �ϳ��� �� Addressable Label ����
        // �װ� ���ҽ� �Ŵ����� ����Ʈ�� ����ֵ�, ��������Ʈ �� ���ϵ� �̷��� �Ἥ ���ε�����
        // �ڽ� ������Ʈ�� base�� ��ӹ޾� �����, ���� �Լ��� ��������ش�.

        GameManager.Inst.Init();
    }

}
