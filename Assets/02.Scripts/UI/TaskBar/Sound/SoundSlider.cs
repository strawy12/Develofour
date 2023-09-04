using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Audio;

public class SoundSlider : MonoBehaviour
{
    public Slider slider;

    public TMP_Text valueText;

    public SoundPanelImage soundImage;
    public SoundPanelImage soundTaskbarImage;

    public string nameStr;

    public float saveSound; //나중에 Json으로 빼서 저장하겠지

    private bool isMute; //ismute가 true면 소리는 0으로

    public AudioMixer audioMixer;
    public ESoundPlayerType soundType;

    public void Init(float _saveSound)
    {
        saveSound = _saveSound;
        slider.value = saveSound;

        SetMixGroup();
        SetValueText(slider);
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
        slider.onValueChanged.AddListener(Setting);
    }

    public void Setting(float temp)
    {
        if (isMute)
        {
            return;
        }
        SetValueText(slider);
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
        SetMixGroup();
        if (soundType == ESoundPlayerType.BGM)
        {
            DataManager.Inst.SaveBGMSoundValue(temp);
        }
        else
        {
            DataManager.Inst.SaveEffectSoundValue(temp);
        }
        DataManager.Inst.SaveDefaultJson();
    }

    public void SetMixGroup()
    { // -20 - 10

        if (slider.value == 0)
        {
            audioMixer.SetFloat(soundType.ToString(), -80);
            return;
        }
        audioMixer.SetFloat(soundType.ToString(), ((slider.value * 35) - 25));
    }

    private void SetValueText(Slider slider)
    {
        if (isMute) return;
        int value = (int)(slider.value * 100);
        valueText.text = value.ToString();
    }

    private void SetSoundImage(Slider slider, SoundPanelImage soundImage)
    {
        float value = slider.value * 100;
        if (value == 0)
        {
            soundImage.ChangeCondition(ESoundCondition.X);
        }
        else if (value > 66)
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
        if (!isMute)
        {
            isMute = true;
            SetMuteSoundImage(slider, soundImage);
        }
        else
        {
            isMute = false;
            Setting(0);
        }
    }

    private void SetMuteSoundImage(Slider slider, SoundPanelImage soundImage)
    {
        soundImage.ChangeCondition(ESoundCondition.X);
        soundTaskbarImage.ChangeCondition(ESoundCondition.X);
        audioMixer.SetFloat(soundType.ToString(), -80);
    }

}
