using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskBar : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TaskIcon taskIconPrefab;
    
    private Dictionary<string,TaskIcon> taskIcons = new Dictionary<string, TaskIcon>();
    private Transform taskIconParent;

    static public Func<string, bool> RemoveIconEvent;
    private void Awake()
    {
        Bind();
        AddFixedTaskIcons();

        EventManager.StartListening(EEvent.CreateWindow, AddTaskIcon);
        RemoveIconEvent += RemoveTaskIcon;
    }

    private void Bind()
    {
        taskIconParent = transform.Find("TaskIcons");
    }

    private void AddFixedTaskIcons()
    {
        TaskIcon[] icons = GetComponentsInChildren<TaskIcon>();

        foreach (TaskIcon icon in icons)
        {
            if (icon is FixedTaskIcon)
            {
                taskIcons.Add(icon.Title,icon);
            }
        }
    }

    public void AddTaskIcon(object param)
    {
        if (param == null || !(param is Window)) { return; }

        Window window = param as Window;
        TaskIcon taskIcon = null;
       
        if (!taskIcons.ContainsKey(window.WindowData.Title))
        {
            taskIcon = CreateTaskIcon();
        }
        else
        {
            taskIcon = taskIcons[window.WindowData.Title];
        }

        taskIcon.AddTargetWindow(window);
    }

    private TaskIcon CreateTaskIcon()
    {
        TaskIcon icon = Instantiate(taskIconPrefab, taskIconParent);
        icon.gameObject.name = icon.gameObject.name.Replace("(Clone)", "");
        icon.Init();

        return icon;
    }

    public bool RemoveTaskIcon(string ID)
    {
        if (taskIcons.ContainsKey(ID))
        {
            taskIcons.Remove(ID);
            return true;
        }
        return false;

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        WindowManager.Inst.SelectedObjectNull();
    }
}
