using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SoundPanel : MonoBehaviour
{
    public Slider BGMSlider;
    public Slider effectSlider;

    public TMP_Text valueText;

    public SoundPanelImage soundImage;

    public void Start()
    {
        Init();    
    }

    public void Init()
    {
        SetValueText(BGMSlider);
        SetSoundImage(BGMSlider);
        BGMSlider.onValueChanged.AddListener(delegate { SetValueText(BGMSlider); SetSoundImage(BGMSlider); });
        //effectSlider.onValueChanged.AddListener(delegate { SetValueText(effectSlider); SetSoundImage(effectSlider); });
    }

    private void SetValueText(Slider slider)
    {
        int value = (int)(slider.value * 100);
        Debug.Log(slider.value);
        valueText.text = value.ToString();
    }

    private void SetSoundImage(Slider slider)
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
