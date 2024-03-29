using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static Constant.ProfilerInfoKey;
using static Constant.MonologKey;

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
    private Image backgroundImage;

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TMP_Text timerText;

    [SerializeField]
    private SelectPuzzle selectPuzzle;

    public override void ShowCutScene()
    {
        base.ShowCutScene();
        CutScene0_Start();
    }

    private void CutScene0_Start()
    {
        selectPuzzle.Init();

        string monologID = Constant.MonologKey.CCTV_CUTSCENE_00;
        MonologSystem.AddOnEndMonologEvent(monologID,CutScene1_Start);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false); ;
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

        string monologID = CCTV_CUTSCENE_01;
        MonologSystem.AddOnEndMonologEvent(monologID, CutScene2_Start);
        MonologSystem.AddOnEndMonologEvent(monologID, () => GetProfilerInfo(CCTV_TIME));
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
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

        string monologID = CCTV_CUTSCENE_02;
        MonologSystem.AddOnEndMonologEvent(monologID, CutScene3_Start);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
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
        string monologID = Constant.MonologKey.CCTV_CUTSCENE_03;

        MonologSystem.AddOnEndMonologEvent(monologID, CutScene4_Start);
        MonologSystem.AddOnEndMonologEvent(monologID, () => GetProfilerInfo(CCTV_DETAIL));
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
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

        string monologData = CCTV_CUTSCENE_04;
        MonologSystem.AddOnEndMonologEvent(monologData, CutScene5_Start);
        MonologSystem.AddOnEndMonologEvent(monologData, () => GetProfilerInfo(CCTV_UYOUNGWHEREABOUTS));
        MonologSystem.OnStartMonolog?.Invoke(monologData, false);
    }


    private void CutScene5_Start()
    {
        StartCoroutine(CutScene5_StartCor());
    }

    private IEnumerator CutScene5_StartCor()
    {
        isCanStop = false;
        backgroundImage.gameObject.SetActive(false);
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1.25f);

        selectPuzzle.Fade(true, 1f);

        yield return new WaitForSeconds(1.25f);

        string monologData = "T_M_10";
        MonologSystem.OnStartMonolog?.Invoke(monologData, false);

        yield return new WaitUntil(() => selectPuzzle.selectInfoTrigger.isClear == true);
        yield return new WaitForSeconds(1f);

        //대강 독백 쓰고 
        StopCutScene();
    }


    private void DelayStart()
    {
        StartCoroutine(DelayStartCor());
    }

    private IEnumerator DelayStartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(3f);
        //EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[] { EProfilerCategory.Bat, Constant.ProfilerInfoKey.BAT_DETAIL });
        //EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[] { EProfilerCategory.CriminalInfomation, Constant.ProfilerInfoKey.CRIMINAL_ACTION });
        StopCutScene();
    }

    public override void StopCutScene()
    {
        isCanStop = true;
        StopAllCoroutines();
        MonologSystem.OnStopMonolog?.Invoke();
        base.StopCutScene();
    }

    private void GetProfilerInfo(string infoID)
    {
        object[] ps = new object[2];
        //ps[0] = ;
        ps[1] = infoID;

        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, ps);
    }
}
