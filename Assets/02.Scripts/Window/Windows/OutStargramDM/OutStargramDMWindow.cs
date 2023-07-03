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
        //나중에 저장 관리해서 로그인 패널 setActive 
        outStarLoginPanel.Init();
    }
    
    
}



