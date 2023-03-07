using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSelectPanel : MonoBehaviour
{
    public Button BGMButton;
    public Button EffectButton;

    public Button SelectButton;

    public SoundChangePanel changePanel;


    public void Init()
    {
        SelectButton.onClick.AddListener(CloseSelectPanel);
        BGMButton.onClick.AddListener(delegate { SliderSetting("BGM ����Ŀ"); });
        EffectButton.onClick.AddListener(delegate { SliderSetting("Effect ����Ŀ"); });
    }

    public void CloseSelectPanel()
    {
        changePanel.ChangeSlider();
        changePanel.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SliderSetting(string str)
    {
        changePanel.currentText.text = str;
        CloseSelectPanel();
        changePanel.ChangeSlider();
    }
}
