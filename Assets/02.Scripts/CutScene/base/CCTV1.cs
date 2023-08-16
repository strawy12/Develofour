using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CCTV1 : CutScene
{
    [SerializeField]
    private Sprite cutSceneSprite01;
    [SerializeField]
    private Sprite cutSceneSprite02;
    [SerializeField]
    private Sprite cutSceneSprite03;
    [SerializeField]
    private Sprite cutSceneSprite04;
    [SerializeField]
    private Sprite cutSceneSprite05;
    [SerializeField]
    private Sprite cutSceneSprite06;

    [SerializeField]
    private Image sprite;

    [SerializeField]
    private TMP_Text timerText;
    [Header("Delay")]
    [SerializeField]
    private float cutdelay_3 = 3f;
    public override void ShowCutScene()
    {
        base.ShowCutScene();
        CutScene1_Start();
    }

    private void CutScene0_Start()
    {
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
        timerText.text = "[2023.10.20 01:10]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene2_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_1, 0, true);
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
        timerText.text = "[2023.10.20 01:16]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene3_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_2, 0, true);
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
        timerText.text = "[2023.10.20 01:18]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(cutdelay_3);
        sprite.DOFade(0, 1);
        sprite.sprite = cutSceneSprite04;
        timerText.text = "[2023.10.20 01:20]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(cutdelay_3);
        sprite.DOFade(0, 1);
        sprite.sprite = cutSceneSprite01;
        timerText.text = "[2023.10.20 01:21]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene4_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_3, 0, true);


    }

    private void CutScene4_Start()
    {
        StartCoroutine(CutScene4_StartCor());
    }

    private IEnumerator CutScene4_StartCor()
    {
        for(int i = 1; i < 5; i++)
        {
            timerText.text = $"[2023.10.20 01:{21+i}]";
            yield return new WaitForSeconds(2f);
        }
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite05;
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene5_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_4, 0, true);
    }

    private void CutScene5_Start()
    {
        StartCoroutine(CutScene5_StartCor());
    }

    private IEnumerator CutScene5_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite06;
        timerText.text = "[2023.10.20 01:26]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);

        MonologSystem.OnEndMonologEvent = CutScene6_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_5, 0, true);
    }

    private void CutScene6_Start()
    {
        StartCoroutine(CutScene6_StartCor());
    }

    private IEnumerator CutScene6_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite01;
        timerText.text = "[2023.10.20 01:27]";
        MonologSystem.OnEndMonologEvent = DelayStart;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_6, 0, true);
    }

    private void DelayStart()
    {
        StartCoroutine(DelayStartCor());
    }

    private IEnumerator DelayStartCor()
    {
        for (int i = 1; i < 4; i++)
        {
            timerText.text = $"[2023.10.20 01:{27 + i}]";
            yield return new WaitForSeconds(2f);
        }
        MonologSystem.OnEndMonologEvent = StopCutScene;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_1_7, 0, true);
    }

    public override void StopCutScene()
    {
        StopAllCoroutines();
        base.StopCutScene();
    }
}
