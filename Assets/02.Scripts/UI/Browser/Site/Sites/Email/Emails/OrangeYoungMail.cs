using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OrangeYoungMail : Mail
{
    [SerializeField]
    private ScrollRect sr;

    public async void SetContentPosition()
    {
        await Task.Delay(200);

        sr.verticalNormalizedPosition = 0;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void ShowMail()
    {
        base.ShowMail();
    }

    public override void HideMail()
    {
        base.HideMail();
    }

    protected override void DestroyMail()
    {
        base.DestroyMail();
    }
}
