using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class SoundSlider : MonoBehaviour    
{
    public Slider slider;

    public TMP_Text valueText;

    public SoundPanelImage soundImage;
    public SoundPanelImage soundTaskbarImage;

    public string nameStr;

    private int save;

    private bool isMute; //ismute가 true면 소리는 0으로

    public void Init()
    {
        SetValueText(slider);
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
        slider.onValueChanged.AddListener(Setting);
    }

    public void Setting(float temp = 0)
    {
        if (isMute)
        {
            isMute = false;
        }
        SetValueText(slider); 
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
    }

    private void SetValueText(Slider slider)
    {
        if (isMute) return;
        int value = (int)(slider.value * 100);
        Debug.Log(slider.value);
        valueText.text = value.ToString();
    }

    private void SetSoundImage(Slider slider, SoundPanelImage soundImage)
    {
        float value = slider.value * 100;
        if(value == 0)
        {
            soundImage.ChangeCondition(ESoundCondition.X);
        }
        else if(value > 66)
        {
            soundImage.ChangeCondition(ESoundCondition.Big);
        }
        else if (value > 33)
        {
            soundImage.ChangeCondition(ESoundCondition.Middle);
        }
        else if (value > 0)
        {
            soundImage.ChangeCondition(ESoundCondition.Small);
        }
    }

    public void Mute()
    {
        if(!isMute)
        {
            isMute = true;
            SetMuteSoundImage(slider, soundImage);
        }
        else
        {
            isMute = false;
            Setting();
        }
    }

    private void SetMuteSoundImage(Slider slider, SoundPanelImage soundImage)
    {
        soundImage.ChangeCondition(ESoundCondition.X);
        soundTaskbarImage.ChangeCondition(ESoundCondition.X);
    }
}
