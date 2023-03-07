using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ESoundCondition
{
    None = -1,
    Big,
    Middle,
    Small,
    X
}

public class SoundPanelImage : MonoBehaviour, IPointerDownHandler
{
    public Image bigSound;
    public Image middleSound;
    public Image smallSound;
    public Image xSound;
    public Image sound;

    public bool isClickMute;
    public bool isSoundPanelOpen;
    public SoundPanel soundPanel;

    public void ChangeCondition(ESoundCondition condition)
    {
        switch(condition)
        {
            case ESoundCondition.Big:
                {
                    SetFade(bigSound, 1);
                    SetFade(middleSound, 1);
                    SetFade(smallSound, 1);
                    xSound.gameObject.SetActive(false);
                }
                break;

            case ESoundCondition.Middle:
                {
                    SetFade(bigSound, 0);
                    SetFade(middleSound, 1);
                    SetFade(smallSound, 1);
                    xSound.gameObject.SetActive(false);
                }
                break;

            case ESoundCondition.Small:
                {
                    SetFade(bigSound, 0);
                    SetFade(middleSound, 0);
                    SetFade(smallSound, 1);
                    xSound.gameObject.SetActive(false);
                }
                break;

            case ESoundCondition.X:
                {
                    SetFade(bigSound, 0);
                    SetFade(middleSound, 0);
                    SetFade(smallSound, 0);
                    xSound.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void SetFade(Image image, float value)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isSoundPanelOpen && !isClickMute)
        {
            soundPanel.gameObject.SetActive(true);
        }
        if(isClickMute)
        {
            soundPanel.Mute();
        }
    }
}
