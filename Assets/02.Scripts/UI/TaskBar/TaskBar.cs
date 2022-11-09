using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour, IPointerClickHandler
{
    static public Action<Window> OnAddIcon;

    [SerializeField]
    private TaskIcon taskIconPrefab;
    [SerializeField]
    private Transform taskIconParent;
    [SerializeField]
    private int poolCnt = 10;
    private Stack<TaskIcon> taskIconPool;
    private Dictionary<int, TaskIcon> taskIcons;

    private void Awake()
    {
        taskIconPool = new Stack<TaskIcon>();
        taskIcons = new Dictionary<int, TaskIcon>();

        CreatePool(poolCnt);
    }

    private void CreatePool(int cnt)
    {
        for (int i = 0; i < cnt; i++)
        {
            TaskIcon taskIcon = Instantiate(taskIconPrefab, taskIconParent);
            taskIconPool.Push(taskIcon);
            taskIcon.gameObject.SetActive(false);
        }
    }

    private void AddFixedIcons()
    {

    }

    public void AddIcon(Window window)
    {

    }

    private TaskIcon CreateTaskIcon()
    {
        if (taskIconPool.Count <= 0)
        {
            CreatePool(1);
        }
        TaskIcon newTaskIcon = taskIconPool.Pop();

        return newTaskIcon;
    }

    public void RemoveTaskIcon(TaskIcon icon)
    {
        taskIconPool.Push(icon);
        icon.CloseIcon();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
