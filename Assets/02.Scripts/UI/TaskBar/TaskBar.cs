using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaskBar : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField]
    private TaskIcon taskIconPrefab;
    
    private Dictionary<int,TaskIcon> taskIcons = new Dictionary<int, TaskIcon>();
    
    [SerializeField]
    private Transform taskIconParent;

    static public Action<object> OnAddIcon; 
    private void Awake()
    {
        Bind();
        AddFixedTaskIcons();

        EventManager.StartListening(EEvent.CreateWindow, AddTaskIcon);
        OnAddIcon += AddTaskIcon; 
    }

    private void Bind() 
    {
         
    }

    private void AddFixedTaskIcons()
    {
        TaskIcon[] icons = GetComponentsInChildren<TaskIcon>();

        foreach (TaskIcon icon in icons)
        {
            if (icon.IsFixed)
            {
                taskIcons.Add(icon.WindowType,icon);
            }
        }
    }

    public void AddTaskIcon(object param)
    {
        if (param == null || !(param is Window)) { return; }

        Window window = param as Window;
        TaskIcon taskIcon = null;
       
        if (!taskIcons.ContainsKey(window.WindowType))
        {
            taskIcon = CreateTaskIcon();
        }
        else
        {
            taskIcon = taskIcons[window.WindowType];
        }

    }

    private TaskIcon CreateTaskIcon()
    {
        TaskIcon icon = Instantiate(taskIconPrefab, taskIconParent);
        icon.gameObject.name = icon.gameObject.name.Replace("(Clone)", "");
        icon.OnDetroy += RemoveTaskIcon;
        icon.Init();
        return icon;
    }

    public void RemoveTaskIcon(int winType)
    {
        if (taskIcons.ContainsKey(winType))
        {
            taskIcons.Remove(winType);
        }
        return;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        WindowManager.Inst.SelectedObjectNull();
    }
}
