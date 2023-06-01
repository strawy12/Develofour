using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CCTVCutScene : CutScene
{
    [SerializeField]
    private Sprite cutSceneSprite00;
    [SerializeField]
    private Sprite cutSceneSprite01;
    [SerializeField]
    private Sprite cutSceneSprite02;
    [SerializeField]
    private Sprite cutSceneSprite03;
    [SerializeField]
    private Sprite cutSceneSprite04;


    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TMP_Text timerText;

    public override void ShowCutScene()
    {
        base.ShowCutScene();
        CutScene0_Start();
    }

    private void CutScene0_Start()
    {
        MonologSystem.OnEndMonologEvent = CutScene1_Start;
        MonologSystem.OnStartMonolog?.Invoke(181, 0, true);
    }

    private void CutScene1_Start()
    {
        StartCoroutine(CutScene1_StartCor());
    }

    IEnumerator CutScene1_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite01;
        timerText.text = "[2023.10.20 01:13]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene2_Start;
        MonologSystem.OnStartMonolog?.Invoke(182, 0, true);
    }

    private void CutScene2_Start()
    {
        StartCoroutine(CutScene2_StartCor());
    }

    private IEnumerator CutScene2_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite02;
        timerText.text = "[2023.10.20 01:20]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene3_Start;
        MonologSystem.OnStartMonolog?.Invoke(183, 0, true);
    }

    private void CutScene3_Start()
    {
        StartCoroutine(CutScene3_StartCor());
    }

    private IEnumerator CutScene3_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite03;
        timerText.text = "[2023.10.20 01:23]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene4_Start;
        MonologSystem.OnStartMonolog?.Invoke(184, 0, true);
    }

    private void CutScene4_Start()
    {
        StartCoroutine(CutScene4_StartCor());
    }

    private IEnumerator CutScene4_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite04;
        timerText.text = "[2023.10.20 01:24]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = DelayStart;
        MonologSystem.OnStartMonolog?.Invoke(185, 0, true);
    }

    private void DelayStart()
    {
        StartCoroutine(DelayStartCor());
    }

    private IEnumerator DelayStartCor()
    {
        yield return new WaitForSeconds(3f);
        StopCutScene();
    }

    public override void StopCutScene()
    {
        base.StopCutScene();
    }
}
