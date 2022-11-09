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

    private Stack<TaskIcon> taskIconPool;
    private Dictionary<int, TaskIcon> taskIcons;

    private void Awake()
    {
        taskIconPool = new Stack<TaskIcon>();
        taskIcons = new Dictionary<int, TaskIcon>();
    }

    private void Start()
    {
        AddFixedIcons();
    }

    private void AddFixedIcons()
    {
        for(int i = 0; i < taskIconParent.childCount; i++)
        {
            //FixedIcon icon = taskIconParent.GetChild(i).GetComponent<FixedIcon>();
            //if(icon != null)
            //{
            //    taskIcons.Add(icon.windowPrefab.windowData.WindowType, icon);
            //}
            
            // key는 자식의 WindowType Enum
            // value는 자식의 taskicon
        }
    }

    public void AddIcon(Window window)
    {
        CreateTaskIcon();

        if(taskIcons.ContainsKey((int)window.WindowData.windowType))
        {

        }
        else
        {
            taskIcons.Add((int)window.WindowData.windowType, window)
        }
    }

    private TaskIcon CreateTaskIcon()
    {
        return null;
    }

    public void RemoveTaskIcon(TaskIcon icon)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
