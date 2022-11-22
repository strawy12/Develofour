using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowManager : MonoSingleton<WindowManager>
{
    private Dictionary<System.Enum, List<Window>> windowDictionary = new Dictionary<System.Enum, List<Window>>();
    private List<Window> windowPrefab = new List<Window>();

    public Window GetWindow(System.Enum windowEnum, int titleId)
    {
        return null;
    }

    public bool IsExistWindow(System.Enum windowEnum)
    {
        return false;
    }

    public void CreateWindow(System.Enum windowEnum, int titleId) 
    {

    }

    public Window GetWindowPrefab(System.Enum windowEnum, int titleId)
    {
        return null;
    }

    private ISelectable selectedObject = null;

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
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);

            object[] ps = new object[1] { hits };
            EventManager.TriggerEvent(ECoreEvent.LeftButtonClick, ps);

            if (selectedObject == null) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.GetComponent<ISelectable>() == selectedObject)
                {
                    return;
                }
            }

            SelectedObjectNull();
        }
    }
}