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
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_CUTSCENE_00, 0, true); ;
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
        MonologSystem.OnEndMonologEvent =
            () => EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { EProfileCategory.CCTV, Constant.ProfileInfoKey.CCTV_TIME });
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_CUTSCENE_01, 0, true);
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
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_CUTSCENE_02, 0, true);
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
        MonologSystem.OnEndMonologEvent =
    () => EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { EProfileCategory.CCTV, Constant.ProfileInfoKey.CCTV_DETAIL }) ;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_CUTSCENE_03, 0, true);
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
        MonologSystem.OnEndMonologEvent =
    () => EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { EProfileCategory.CCTV, Constant.ProfileInfoKey.CCTV_UYOUNGWHEREABOUTS });
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_CUTSCENE_04, 0, true);
    }

    private void DelayStart()
    {
        StartCoroutine(DelayStartCor());
    }

    private IEnumerator DelayStartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(3f);
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { EProfileCategory.Bat, Constant.ProfileInfoKey.BAT_DETAIL });
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[] { EProfileCategory.CriminalInfomation, Constant.ProfileInfoKey.CRIMINAL_ACTION });
        StopCutScene();
    }

    public override void StopCutScene()
    {
        StopAllCoroutines();
        MonologSystem.OnStopMonolog?.Invoke();
        base.StopCutScene();
    }
}
