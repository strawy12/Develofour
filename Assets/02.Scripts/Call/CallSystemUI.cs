using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallSystemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image profileIcon;

    [SerializeField]
    private Button answerBtn;
    [SerializeField]
    private AudioSpectrumUI spectrumUI;

    private bool isRecieveCall;

    [SerializeField]
    private Transform selectButtonParent;
    [SerializeField]
    private CallSelectButton selectButton;

    [SerializeField]
    private GameObject callCoverPanel;

    private List<CallSelectButton> buttonList;
    private bool isCalling;


    public void SetCallUI(CallProfileDataSO data)
    {
        if (data.characterName == "")
        {
            nameText.text = data.phoneNum;
        }
        else
        {
            nameText.text = data.characterName;
        }
        profileIcon.sprite = data.profileIcon;
    }

    private IEnumerator PhoneSoundCor()
    {
        if (isCalling)
        {
            yield return new WaitUntil(() => !isCalling);
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(4f);
        }

        isRecieveCall = false;
    }


}
