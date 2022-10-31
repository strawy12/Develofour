using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    private ISelectable selectedObject = null;

    public void SelectObject(ISelectable target)
    {
        Debug.Log(11);
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = target;
        selectedObject?.OnSelected?.Invoke();
    }

    public void SelectedObjectNull()
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = null;
    }
}
