using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinderCallAnswerUI : MonoBehaviour
{
    [SerializeField]
    private Button callAnswerBtn;

    private bool isRecieveCall;

    public void PlayCall(ECharacterType characterType)
    {
        StartCoroutine(PlayPhoneSoundAndShake());
    }

    private IEnumerator PlayPhoneSoundAndShake()
    {
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(soundSecond + Constant.PHONECALLSOUND_DELAY);
        }
        isRecieveCall = false;
    }
}
