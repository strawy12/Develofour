using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartCutScene1 : MonoBehaviour
{
    public TMP_Text titleText;

    public Image interrogationRoomSprite;

    void Start()
    {
        EventManager.StartListening(ECoreEvent.EndDataLoading, CutSceneStart);
    }

    private void CutSceneStart(object[] ps)
    {
        StartCoroutine(CutSceneStart());
    }

    private IEnumerator CutSceneStart()
    {
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneScream);

        yield return new WaitForSeconds(15f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutScenePoint);
        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        titleText.DOColor(new Color(255, 255, 255, 0), 2.5f);
        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(1f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartCutSceneLightPull);
        interrogationRoomSprite.DOFade(1, 2f);
        yield return new WaitForSeconds(1.5f);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.InterrogationRoom);

        MonologSystem.OnEndMonologEvent += FadeInterrogationRoomSprite;
        MonologSystem.OnStartMonolog(EMonologTextDataType.StartCutSceneMonolog1, 0, true);
    }

    private void FadeInterrogationRoomSprite()
    {
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.InterrogationRoom);
        interrogationRoomSprite.DOFade(0, 1.5f);
    }
}
