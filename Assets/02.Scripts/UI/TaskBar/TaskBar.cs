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

    private void AddFixedIcons()
    {

    }

    public void AddIcon(Window window)
    {

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
