using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrunchSite : Site
{
    [SerializeField]
    private GameObject WriterInfoPanel;
    [SerializeField]
    private GameObject PostListPanel;
    [SerializeField]
    private GameObject WorkListPanel;

    [SerializeField]
    private List<BruchWriteDataSO> writeDatas = new List<BruchWriteDataSO>();

    public override void Init()
    {
        base.Init();
    }
        
    protected override void ResetSite()
    {
        base.ResetSite();
    }

    protected override void ShowSite()
    {
        base.ShowSite();
    }
}
