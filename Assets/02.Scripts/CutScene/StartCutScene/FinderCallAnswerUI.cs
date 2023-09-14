using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FinderCallAnswerUI : MonoBehaviour
{
    [SerializeField]
    private Button callAnswerBtn;
    [SerializeField]
    private List<Sprite> characterSpriteList;
    [SerializeField]
    private Image profileImage;
    [SerializeField]
    private TMP_Text characterText;

    public Action OnClick;

    private bool isRecieveCall;

    public void Show()
    {
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void SetCharacter(ECharacterType characterType)
    {
        switch (characterType)
        {
            case ECharacterType.Assistant:
                profileImage.sprite = characterSpriteList[0];
                characterText.text = "����";
                break;

            case ECharacterType.Police:
                profileImage.sprite = characterSpriteList[1];
                characterText.text = "����";
                break;

            default:
                Debug.LogError("�ش����� �ʴ� ĳ���� Ÿ���� �Ķ���ͷ� �ۼ��߽��ϴ�" + characterType.ToString());
                return;
        }
    }

    public void PlayCall()
    {
        callAnswerBtn.onClick.AddListener(OnClickBtn);
        StartCoroutine(PlayPhoneSoundAndShake());
    }

    private void OnClickBtn()
    {
        isRecieveCall = false;
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
        OnClick?.Invoke();

        OnClick = null;
    }

    private IEnumerator PlayPhoneSoundAndShake()
    {
        transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.7f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);

            if(GameManager.Inst.GameState != EGameState.Text)
            {
                float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
                yield return new WaitForSeconds(soundSecond + Constant.PHONECALLSOUND_DELAY);
            }
            else
            {
                yield return new WaitForSeconds(4f);
            }
        }
        isRecieveCall = false;
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
