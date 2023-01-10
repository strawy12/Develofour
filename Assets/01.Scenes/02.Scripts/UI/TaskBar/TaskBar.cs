using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour
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

    private void Start()
    {
        //AddFixedIcons();
    }

    private void AddFixedIcons()
    {
        for(int i = 0; i < taskIconParent.childCount; i++)
        {
            FixedTaskIcon icon = taskIconParent.GetChild(i).GetComponent<FixedTaskIcon>();
            if (icon != null)
            {
                taskIcons.Add((int)icon.WindowPrefab.WindowData.windowType, icon);
            }

            // key는 자식의 WindowType Enum
            // value는 자식의 taskicon
        }
    }

    public void AddIcon(Window window)
    {
        if(!taskIcons.ContainsKey((int)window.WindowData.windowType))
        {
            TaskIcon taskIcon = CreateTaskIcon();
            taskIcon.Init((int)window.WindowData.windowType);
            taskIcon.gameObject.SetActive(true);
            taskIcon.AddWindow(window);
        }
        else
        {
            taskIcons.Add((int)window.WindowData.windowType, taskIcons[(int)window.WindowData.windowType]);
        }
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
}
