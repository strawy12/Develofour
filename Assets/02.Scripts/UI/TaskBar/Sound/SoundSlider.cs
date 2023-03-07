using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SoundSlider : MonoBehaviour    
{
    public Slider slider;

    public TMP_Text valueText;

    public SoundPanelImage soundImage;
    public SoundPanelImage soundTaskbarImage;

    public string nameStr;

    public void Init()
    {
        SetValueText(slider);
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
        slider.onValueChanged.AddListener(Setting);
    }

    public void Setting(float temp = 0)
    {
        SetValueText(slider); 
        SetSoundImage(slider, soundImage);
        SetSoundImage(slider, soundTaskbarImage);
    }

    private void SetValueText(Slider slider)
    {
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
}
