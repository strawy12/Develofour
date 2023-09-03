using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;



[RequireComponent(typeof(Volume))]
public class VolumeHandler : MonoBehaviour
{
    [SerializeField]
    private VolumeProfile titleData;
    [SerializeField]
    private VolumeProfile cutSceneData;
    private Volume volume;

    void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
        this.gameObject.SetActive(false);
    }

    private void Init()
    {
        volume = GetComponent<Volume>();
        GameManager.Inst.OnChangeGameState += ChangeVolumeData;
        Debug.Log("ASdfsdafsdaf");
        EventManager.StartListening(ECoreEvent.OpenVolume, ChangeVolumeData);
    }

    private void ChangeVolumeData(EGameState state)
    {
        switch (state)
        {
            case EGameState.PlayTitle:
                volume.profile = titleData;
                break;

            case EGameState.Game:
            case EGameState.CutScene:

                break;
        }
    }

    private void ChangeVolumeData(object[] ps)
    {
        Debug.Log("sadfsdafasdf");
        if (!(ps[0] is bool))
            return;

        bool isOpen = (bool)ps[0];

        if (isOpen)
        {
            volume.profile = cutSceneData;
        }

        volume.gameObject.SetActive(isOpen);
    }
}