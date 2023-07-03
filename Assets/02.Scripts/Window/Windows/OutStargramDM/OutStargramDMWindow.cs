using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStargramDMWindow : Window
{
    [SerializeField]
    private OutStarUserListPanel outStarUserPanel;

    [SerializeField]
    private OutStarLoginPanel outStarLoginPanel;
    [SerializeField]
    private ChattingPanel chattingPanel;
    //[SerializeField]
    //private 
    protected override void Init()
    {
        base.Init();
        outStarUserPanel.Init();
        if(!DataManager.Inst.SaveData.isOutStarLogin)
        {
            outStarLoginPanel.Init();
        }
        else
        {
            outStarLoginPanel.gameObject.SetActive(false);
        }
        chattingPanel.Init();
    }
    
    
}

