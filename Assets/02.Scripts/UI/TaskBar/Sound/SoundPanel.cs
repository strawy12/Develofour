using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPanel : MonoUI
{

    public SoundSlider bgm;
    public SoundSlider effect;
    public SoundChangePanel changePanel;
    public SoundSelectPanel selectPanel;

    public ContentSizeFitter csf;


    private bool isOpen;

    void Start()
    {
        Init();
        this.SetActive(false);
    }

    public void Init()
    {
        bgm.Init();
        effect.Init();
        effect.gameObject.SetActive(false);
        changePanel.Init();
        selectPanel.Init();
        selectPanel.gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        isOpen = true;
        SetActive(true);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }
    private void CheckClose(object[] hits)
    {

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csf.transform);
        if (!isOpen) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false || Define.ExistInHits(changePanel.gameObject, hits[0]) || Define.ExistInHits(selectPanel.gameObject, hits[0]))
        {
            Close();
        }
    }

    public void Close()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
        isOpen = false;
        SetActive(false);
    }
}
