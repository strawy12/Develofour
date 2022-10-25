using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBar : MonoBehaviour
{
    [SerializeField]
    private TaskIcon taskIconPrefab;

    private List<TaskIcon> taskIcons = new List<TaskIcon>();
    private Transform taskIconParent;

    private void Awake()
    {
        Bind();
        AddFixedTaskIcons();

        EventManager.StartListening(EEvent.CreateWindow, AddTaskIcon);
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
                taskIcons.Add(icon);
            }
        }
    }

    public void AddTaskIcon(object param)
    {
        if (param == null || !(param is Window)) { return; }

        Window window = param as Window;
        TaskIcon taskIcon = null;
       
        foreach (TaskIcon icon in taskIcons)
        {
            if (icon.Title.Equals(window.WindowData.Title))
            {
                taskIcon = icon;
                break;
            }
        }

        taskIcon ??= CreateTaskIcon();


        taskIcon.AddTargetWindow(window);
    }

    private TaskIcon CreateTaskIcon()
    {
        TaskIcon icon = Instantiate(taskIconPrefab, taskIconParent);
        icon.gameObject.name = icon.gameObject.name.Replace("(Clone)", "");
        icon.Init();

        return icon;
    }

}
