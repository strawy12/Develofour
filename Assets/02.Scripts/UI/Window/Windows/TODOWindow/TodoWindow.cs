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

    private void Start()
    {
        foreach(TodoData data in todoDataList)
        {
            TodoPanel panel = Instantiate(todoPanelPrefab, todoPanelParent);
            panel.Init(data);
            panel.gameObject.SetActive(true);
        }
    }
}
