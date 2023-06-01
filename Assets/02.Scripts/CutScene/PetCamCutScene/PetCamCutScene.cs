using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
        MonologSystem.OnEndMonologEvent = ByTheCollarImageFadeIn;
        StartMonolog(Constant.MonologKey.PetCamMonolog_1);
    }

    private void ByTheCollarImageFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(byTheCollarImage.DOFade(1, byTheCollarImageFadeInDelay));
        sequence.AppendCallback(() => ByTheCollarMonologStart());
    }

    private void ByTheCollarMonologStart()
    {
        MonologSystem.OnEndMonologEvent = PetKickImageFadeIn;
        StartMonolog(Constant.MonologKey.PetCamMonolog_2);
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
        MonologSystem.OnEndMonologEvent = FallenPetImageFadeIn;
        StartMonolog(Constant.MonologKey.PetCamMonolog_3);
    }

    private void FallenPetImageFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(fallenPetImage.DOFade(1, fallenPetImageFadeInDelay));
        sequence.AppendCallback(() => FallenPetMonologStart());
    }

    private void FallenPetMonologStart()
    {
        MonologSystem.OnEndMonologEvent = () => StartCoroutine(ExitSoundCoroutine());
        StartMonolog(Constant.MonologKey.PetCamMonolog_4);
    }

    private IEnumerator ExitSoundCoroutine()
    {
        float soundLength = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.ExitDoor);
        yield return new WaitForSeconds(soundLength + exitSoundAfterDelay);
        StopCutScene();
    }

    public override void StopCutScene()
    {
        DOTween.KillAll();
        base.StopCutScene();
    }
}
