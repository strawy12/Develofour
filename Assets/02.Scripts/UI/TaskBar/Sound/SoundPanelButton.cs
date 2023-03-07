using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanelButton : MonoBehaviour
{
    public Button button;
    public SoundPanel soundPanel;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        button.onClick.AddListener(Open);
    }

    public void Open()
    {
        soundPanel.OpenPanel();
    }
}
