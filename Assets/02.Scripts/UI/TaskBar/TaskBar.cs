using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskBar : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField]
    private TaskIcon taskIconPrefab;
    
    private Dictionary<int,TaskIcon> taskIcons = new Dictionary<int, TaskIcon>();
    
    [SerializeField]
    private Transform taskIconParent;

    static public Action<Window> OnAddIcon;

    private void Awake()
    {
        Bind();
        AddFixedTaskIcons();

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

    public void AddTaskIcon(Window window)
    { 
        TaskIcon taskIcon;

        if (!taskIcons.ContainsKey((int)window.WindowData.windowType))
        {
            taskIcon = CreateTaskIcon();
        }
        else
        {
            taskIcon = taskIcons[(int)window.WindowData.windowType];
        }
        taskIcon.AddTargetWindow(window);
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
