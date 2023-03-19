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

    private Dictionary<EWindowType, TaskIcon> taskIcons = new Dictionary<EWindowType, TaskIcon>();

    private void Awake()
    {
        taskIconPool = new Stack<TaskIcon>();

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
                //taskIcons.Add(, icon);
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
        
        if(ps.Length >= 2 && (ps[1] is EWindowType))
        {
            AddIcon(ps[0] as Window, (EWindowType)ps[1]);
            return;
        }


        AddIcon(ps[0] as Window);
    }
    public void AddIcon(Window window)
    {
 
        if(taskIcons.ContainsKey(window.File.windowType))
        {
            taskIcons[window.File.windowType].AddTargetPanel(window);
        }
        else
        {
            // 생성되어있는 TaskIcon에 해당 윈도우를 담는 아이콘이 존재하는지 여부
            TaskIcon taskIcon;

            taskIcon = CreateTaskIcon();

            taskIcon.Init(window.File);
            taskIcon.OnClose += RemoveTaskIcon;
            taskIcon.gameObject.SetActive(true);

            taskIcon.AddWindow(window);
            taskIcons.Add(window.File.windowType, taskIcon);
        }
    }
    public void AddIcon(Window window, EWindowType windowType)
    {
        if (windowType == EWindowType.IconProperty || windowType == EWindowType.Directory)
        {
            if (taskIcons.ContainsKey(EWindowType.Directory))
            {
                taskIcons[EWindowType.Directory].AddTargetPanel(window);
                return;
            }
        }
        else if (taskIcons.ContainsKey(windowType))
        {
            taskIcons[windowType].AddTargetPanel(window);
            return;
        }

        TaskIcon taskIcon;
        taskIcon = CreateTaskIcon();

        taskIcon.Init(window.File, windowType);
        taskIcon.OnClose += RemoveTaskIcon;
        taskIcon.gameObject.SetActive(true);

        taskIcon.AddWindow(window);
        taskIcons.Add(windowType, taskIcon);


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
        taskIcons.Remove(icon.WindowType);
        icon.CloseIcon();
    }
}
