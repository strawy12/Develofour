using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    private ISelectable selectedObject = null;

    private bool isTaskIconAttribute = false;
    public bool IsTaskIconAttribute
    {
        get { return isTaskIconAttribute; }
        set { isTaskIconAttribute = value; }
    }

    public void SelectObject(ISelectable target)
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = target;
        selectedObject?.OnSelected?.Invoke();
    }

    public void SelectedObjectNull()
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = null;
    }

    void Update()
    {
        if(isTaskIconAttribute == false && selectedObject == null)
        {
            return;
        }
        if(Input.GetMouseButtonDown(0))
        {
            EventManager.TriggerEvent(EEvent.LeftButtonClick);
        }
    }
}
