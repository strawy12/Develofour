using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMail : Mail
{
    private bool isFinish = false;

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

    public override void DestroyMail()
    {
        if (isFinish) return;

        base.DestroyMail();
    }

    public override void FavoriteMail()
    {

    }
}
