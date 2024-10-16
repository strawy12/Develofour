﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static Constant;

public class PetCamCutScene : CutScene
{
    #region Delay
    private float unlockSoundBeforeDelay = 2f;
    private float unlockSoundAfterDelay = 1f;
    private float quarreImageFadeInDelay = 1f;
    private float byTheCollarImageFadeInDelay = 1f;
    private float petKickImageFadeInDelay = 1f;
    private float fallenPetImageFadeInDelay = 1f;
    private float exitSoundAfterDelay = 1f;
    #endregion

    #region GameObject
    [Header("GameObject")]
    [SerializeField] private GameObject blackImagePanel;
    [SerializeField] private Image quarrelImage;
    [SerializeField] private Image byTheCollarImage;
    [SerializeField] private Image petKickImage;
    [SerializeField] private Image fallenPetImage;
    #endregion
    public override void ShowCutScene()
    {
        base.ShowCutScene();
        StartCoroutine(UnlockDoor());
    }
    private IEnumerator UnlockDoor()
    {
        blackImagePanel.SetActive(true);

        yield return new WaitForSeconds(unlockSoundBeforeDelay);
        float soundLength = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.DoorLock);
        yield return new WaitForSeconds(soundLength + unlockSoundAfterDelay);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(quarrelImage.DOFade(1, quarreImageFadeInDelay));
        sequence.AppendCallback(() => QurrelMonologStart());
    }

    private void QurrelMonologStart()
    {
        string monologID = MonologKey.PETCAM_CUTSCENE_1;
        MonologSystem.AddOnEndMonologEvent(monologID, ByTheCollarImageFadeIn);
        StartMonolog(monologID);
    }

    private void ByTheCollarImageFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(byTheCollarImage.DOFade(1, byTheCollarImageFadeInDelay));
        sequence.AppendCallback(() => ByTheCollarMonologStart());
    }

    private void ByTheCollarMonologStart()
    {
        string monologID = MonologKey.PETCAM_CUTSCENE_2;
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { ProfilerCategoryKey.PETCAM, "" });
        MonologSystem.AddOnEndMonologEvent(monologID, PetKickImageFadeIn);
        StartMonolog(monologID);
    }

    private void PetKickImageFadeIn()
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(petKickImage.DOFade(1, petKickImageFadeInDelay));
        sequence.AppendCallback(() => StartCoroutine(PetKickSoundCoroutine()));
    }

    private IEnumerator PetKickSoundCoroutine()
    {
        float kickDelay = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.Kick);
        float whineDelay = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.DogWhine);

        yield return new WaitForSeconds(Mathf.Max(kickDelay, whineDelay));

        PetKickMonologStart();
    }

    private void PetKickMonologStart()
    {
        string monologID = Constant.MonologKey.PETCAM_CUTSCENE_3;
        MonologSystem.AddOnEndMonologEvent(monologID, FallenPetImageFadeIn);
        StartMonolog(monologID);
    }

    private void FallenPetImageFadeIn()
    {
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { ProfilerCategoryKey.PETCAM, "" });

        Sequence sequence = DOTween.Sequence();
        sequence.Append(fallenPetImage.DOFade(1, fallenPetImageFadeInDelay));
        sequence.AppendCallback(() => FallenPetMonologStart());
    }

    private void FallenPetMonologStart()
    {
        string monologID = Constant.MonologKey.PETCAM_CUTSCENE_4;
        MonologSystem.AddOnEndMonologEvent(monologID, () => StartCoroutine(ExitSoundCoroutine()));
        StartMonolog(monologID);
    }

    private IEnumerator ExitSoundCoroutine()
    {
        float soundLength = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.ExitDoor);
        yield return new WaitForSeconds(soundLength + exitSoundAfterDelay);
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { ProfilerCategoryKey.PETCAM, "" });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { ProfilerCategoryKey.PETCAM, "" });
        StopCutScene();
    }

    public override void StopCutScene()
    {
        DOTween.KillAll();
        base.StopCutScene();
    }
}
