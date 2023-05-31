using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PetCamCutScene : CutScene
{
    #region Delay
    [Header("Delay")]
    [SerializeField] private float unlockSoundBeforeDelay = 3f;
    [SerializeField] private float unlockSoundAfterDelay = 1.5f;
    [SerializeField] private float quarreImageFadeInDelay = 1.5f;
    [SerializeField] private float byTheCollarImageFadeInDelay = 1.5f;
    [SerializeField] private float petKickImageFadeInDelay = 1.5f;
    [SerializeField] private float fallenPetImageFadeInDelay = 1.5f;
    [SerializeField] private float exitSoundAfterDelay = 1.5f;
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
        StartCoroutine(UnlockDoar());
    }
    private IEnumerator UnlockDoar()
    {
        blackImagePanel.SetActive(true);
        yield return new WaitForSeconds(unlockSoundBeforeDelay);
        //sound play doarlock unlock sound
        yield return new WaitForSeconds(unlockSoundAfterDelay);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(quarrelImage.DOFade(1, quarreImageFadeInDelay));
        sequence.AppendCallback(() => QurrelMonologStart());
    }

    private void QurrelMonologStart()
    {
        MonologSystem.OnEndMonologEvent = () =>ByTheCollarImageFadeIn();
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
        MonologSystem.OnEndMonologEvent = () => PetKickImageFadeIn();
        StartMonolog(Constant.MonologKey.PetCamMonolog_2);
    }

    private void PetKickImageFadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(petKickImage.DOFade(1, petKickImageFadeInDelay));
        sequence.AppendCallback(() => PetKickMonologStart());
    }

    private void PetKickMonologStart()
    {
        MonologSystem.OnEndMonologEvent = () => FallenPetImageFadeIn();
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
        //sound Play exitSound
        yield return new WaitForSeconds(exitSoundAfterDelay);
        StopCutScene();
    }

    public override void StopCutScene()
    {
        DOTween.KillAll();
        MonologSystem.OnStopMonolog?.Invoke();
        base.StopCutScene();
    }
}
