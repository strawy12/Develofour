using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutStargramDMWindow : Window
{
    [SerializeField]
    private OutStarUserListPanel outStarUserPanel;

    [SerializeField]
    private OutStarLoginPanel outStarLoginPanel; 

    //[SerializeField]
    //private 
    protected override void Init()
    {
        base.Init();
        outStarUserPanel.Init();
        //���߿� ���� �����ؼ� �α��� �г� setActive 
        outStarLoginPanel.Init();
    }
    
    
}



