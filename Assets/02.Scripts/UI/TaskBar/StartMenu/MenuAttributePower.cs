using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributePower : MenuAttributePanel, IPointerClickHandler
{
    [SerializeField]
    private PowerPanel powerPanel;

    private bool isClick = false;

    protected override void Init()
    {
        EventManager.StartListening(EEvent.ActivePowerPanel, (obj) => isClick = (bool)obj);
        base.Init();    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isClick = true;
        SelectedPanel(false);
        powerPanel.Show();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isClick) return;

        base.OnPointerEnter(eventData);
    }
}
