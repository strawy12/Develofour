using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTV2 : CutScene
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
    private float cutdelay_4 = 3f;
    public override void ShowCutScene()
    {
        base.ShowCutScene();
        CutScene1_Start();
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
        timerText.text = "[2023.10.20 01:30]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene2_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_1, 0, true);
    }

    private void CutScene2_Start()
    {
        StartCoroutine(CutScene2_StartCor());
    }

    private IEnumerator CutScene2_StartCor()
    {
        for (int i = 1; i < 11; i++)
        {
            timerText.text = $"[2023.10.20 01:{30 + i}]";
            yield return new WaitForSeconds(1f);
        }
        MonologSystem.OnEndMonologEvent = CutScene3_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_2, 0, true);
    }

    private void CutScene3_Start()
    {
        StartCoroutine(CutScene3_StartCor());
    }

    private IEnumerator CutScene3_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite02;
        timerText.text = "[2023.10.20 01:50]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);

        MonologSystem.OnEndMonologEvent = CutScene4_NewStart;
        MonologSystem.OnEndMonologEvent = () => EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.CCTV_2, 171 });
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_3, 0, true);
    }

    private void CutScene4_NewStart()
    {
        StartCoroutine(CutScene4_New_StartCor());

    }

    private IEnumerator CutScene4_New_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite03;
        timerText.text = "[2023.10.20 01:52]";
        sprite.DOFade(1, 1);
        MonologSystem.OnEndMonologEvent = CutScene4_Start;
        MonologSystem.OnStartMonolog?.Invoke(242, 0, true);
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
        timerText.text = "[2023.10.20 01:53]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(cutdelay_4);
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite01;
        timerText.text = "[2023.10.20 01:55]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        MonologSystem.OnEndMonologEvent = CutScene5_Start;
        MonologSystem.OnEndMonologEvent = () => EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.CCTV_2, 172 });

        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_4, 0, true);
    }

    private void CutScene5_Start()
    {
        StartCoroutine(CutScene5_StartCor());
    }

    private IEnumerator CutScene5_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite05;
        timerText.text = "[2023.10.20 02:05]";
        sprite.DOFade(1, 1);
        yield return new WaitForSeconds(1f);

        MonologSystem.OnEndMonologEvent = CutScene6_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_5, 0, true);
    }

    private void CutScene6_Start()
    {
        StartCoroutine(CutScene6_StartCor());
    }

    private IEnumerator CutScene6_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite06;
        timerText.text = "[2023.10.20 02:07]";
        MonologSystem.OnEndMonologEvent = CutScene7_Start;
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_6, 0, true);
    }
    private void CutScene7_Start()
    {
        StartCoroutine(CutScene7_StartCor());
    }

    private IEnumerator CutScene7_StartCor()
    {
        sprite.DOFade(0, 1);
        yield return new WaitForSeconds(1f);
        sprite.sprite = cutSceneSprite01;
        timerText.text = "[2023.10.20 02:10]";
        MonologSystem.OnEndMonologEvent = StopCutScene;
        MonologSystem.OnEndMonologEvent = () => EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.CCTV_2, 173 });
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.CCTV_2_7, 0, true);
    }
    public override void StopCutScene()
    {
        StopAllCoroutines();
        base.StopCutScene();
    }
}
