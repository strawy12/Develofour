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
    public GameObject taskbarSoundImage;

    private bool isOpen;

    void Start()
    {
        Init();
        this.SetActive(false);
    }

    public void Init()
    {
        bgm.SetMixGroup();
        effect.SetMixGroup();

        bgm.Init();
        effect.Init();
        effect.gameObject.SetActive(false);
        changePanel.Init();
        selectPanel.Init();
        selectPanel.gameObject.SetActive(false);
    }

    public void Mute()
    {
        changePanel.Mute();
    }

    public void OpenPanel()
    {
        //isOpen = true;
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        SetActive(true);
    }

    private void CheckClose(object[] hits)
    {
        if (!isOpen) { isOpen = true; return; }

        if (selectPanel.gameObject.activeSelf)
        {
            if (Define.ExistInHits(gameObject, hits[0]) == false && Define.ExistInHits(selectPanel.gameObject, hits[0]) == false 
                && Define.ExistInHits(changePanel.gameObject, hits[0]) == false && Define.ExistInHits(taskbarSoundImage, hits[0]) == false)
            {
                Close();
            }
        }
        else
        {
            if(Define.ExistInHits(gameObject, hits[0]) == false)
            {
                Close();
            }
        }
       
    }
    
    public void Close()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
        selectPanel.CloseSelectPanel();
        isOpen = false;
        SetActive(false);
    }
}
