using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundChangePanel : MonoBehaviour
{
    public SoundSelectPanel selectPanel;

    public Button changeButton;

    public TMP_Text currentText;

    public SoundSlider BGMSlider;
    public SoundSlider EffectSlider;

    public GameObject coverPanel;

    public void OpenSelectPanel()
    {
        selectPanel.gameObject.SetActive(true);
        coverPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Init()
    {
        changeButton.onClick.AddListener(OpenSelectPanel);
    }

    public void ChangeSlider()
    {
        if(currentText.text == BGMSlider.nameStr)
        {
            BGMSlider.gameObject.SetActive(true);
            EffectSlider.gameObject.SetActive(false);
            BGMSlider.Setting(DataManager.Inst.DefaultSaveData.BGMSoundValue);
        }
        if(currentText.text == EffectSlider.nameStr)
        {
            BGMSlider.gameObject.SetActive(false);
            EffectSlider.gameObject.SetActive(true);
            EffectSlider.Setting(DataManager.Inst.DefaultSaveData.EffectSoundValue);
        }
        coverPanel.SetActive(false);
    }

    public void Mute()
    {
        if (currentText.text == BGMSlider.nameStr)
        {
            BGMSlider.Mute();
        }
        if (currentText.text == EffectSlider.nameStr)
        {
            EffectSlider.Mute();
        }
    }
}
