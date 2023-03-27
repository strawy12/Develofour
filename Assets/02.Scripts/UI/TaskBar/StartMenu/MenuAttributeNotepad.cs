using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributeNotepad : MenuAttributePanel, IPointerClickHandler
{

    private bool isClick = false;

    protected override void Init()
    {
        base.Init();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClick = true;
        SelectedPanel(false);
        EventManager.TriggerEvent(EWindowEvent.CloseAttribute);
        WindowManager.Inst.WindowOpen(EWindowType.Notepad);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isClick)
        {
            isClick = false;
            return;
        }
        base.OnPointerEnter(eventData);
    }
}
