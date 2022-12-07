using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacebookSite : Site
{
    [Header("Pid")]
    [SerializeField]
    private List<FacebookPidPanelDataSO> pidList;
    [SerializeField]
    private Transform pidParent;

    [SerializeField]
    private FacebookPidPanel pidPrefab;

    //Pid부분은 나중에 다시 만들어야함

    private void CreatePid()
    {
        //use Pooling!
        //for(int i = 0; i < pidList.Count; i++)
        //{
        //    FacebookPidPanel pid = Instantiate(pidPrefab, pidParent);
        //    pid.pidDataSO = pidList[i];
        //    pid.Init();
        //    pid.gameObject.SetActive(true);
        //}
    }

    public override void Init()
    {
        CreatePid();
        base.Init();
    }

    protected override void HideSite()
    {
        base.HideSite();
    }

    protected override void ResetSite()
    {
        base.ResetSite();
    }

    protected override void ShowSite()
    {
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Facebook, Constant.LOADING_DELAY });
        base.ShowSite();
    }
}
