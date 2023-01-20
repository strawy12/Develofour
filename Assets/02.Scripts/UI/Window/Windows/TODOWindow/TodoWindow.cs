using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodoWindow : Window
{
    [SerializeField]
    private TodoPanel todoPanelPrefab;
    [SerializeField]
    private Transform todoPanelParent;
    [SerializeField]
    private List<TodoData> todoDataList;

    [SerializeField]
    private Transform expendPanelTrm;

    private void Start()
    {
        foreach(TodoData data in todoDataList)
        {
            TodoPanel panel = Instantiate(todoPanelPrefab, todoPanelParent);
            panel.Init(data);
            //panel.OnClick = () => ShowExpendPanel();

            panel.gameObject.SetActive(true);
        }
    }

    // TODO
    // ���� ExpendPanel ��ũ��Ʈ�� �ű� ����
   
}
