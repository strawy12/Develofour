using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Test1CutScene : CutScene
{


    public GameObject obj;

    public override void ShowCutScene()
    {
        base.ShowCutScene();
        this.gameObject.SetActive(true);
        StartCoroutine(TextCor());
    }

    public IEnumerator TextCor()
    {
        obj.transform.DOMoveX(200, 1f);
        yield return new WaitForSeconds(2f);
        obj.transform.DOMoveX(0, 1f);
        yield return new WaitForSeconds(1.5f);
        EndSetting();
    }

    private void EndSetting()
    {
        //컷씬 봣다고 등등
        //그 외 등등
        StopCutScene();
    }

    public override void StopCutScene()
    {
        //삭제
        base.StopCutScene();
    }

}
