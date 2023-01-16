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

    // 
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
        EventManager.StartListening(EWindowEvent.CreateWindow, AddIcon);
    }

    private void AddFixedIcons()
    {
        for (int i = 0; i < taskIconParent.childCount; i++)
        {
            FixedTaskIcon icon = taskIconParent.GetChild(i).GetComponent<FixedTaskIcon>();
            if (icon != null)
            {
                //taskIcons.Add((int)icon.WindowPrefab.WindowData.windowType, icon);
            }

            // key는 자식의 WindowType Enum
            // value는 자식의 taskicon
        }
    }

    public void AddIcon(object[] ps)
    {
        if (ps == null || ps.Length <= 0 || !(ps[0] is Window))
        {
            Debug.LogError("TaskBar Icon Add Failed");
            return;
        }

        AddIcon(ps[0] as Window);
    }

    public void AddIcon(Window window)
    {
        // 생성되어있는 TaskIcon에 해당 윈도우를 담는 아이콘이 존재하는지 여부

        TaskIcon taskIcon;

        Debug.Log(window.WindowData.windowType);
        // 존재하지 않다면
        if (taskIcons.TryGetValue((int)window.WindowData.windowType, out taskIcon) == false)
        {
            taskIcon = CreateTaskIcon();

            taskIcon.Init(window.WindowData);
            taskIcon.OnClose += RemoveTaskIcon;
            taskIcon.gameObject.SetActive(true);
            taskIcons.Add((int)window.WindowData.windowType, taskIcon);
        }

        taskIcon.AddWindow(window);
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
        icon.gameObject.SetActive(false);
        taskIconPool.Push(icon);
        icon.CloseIcon();
    }
}
