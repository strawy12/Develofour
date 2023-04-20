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


    private void Start()
    {
        volume = GetComponent<Volume>();
        GameManager.Inst.OnChangeGameState += ChangeVolumeData;
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
            case EGameState.Tutorial:
                volume.profile = cutSceneData;
                break;
        }
    }

    private void ChangeVolumeData(object[] ps)
    {
        if (ps != null && !(ps[0] is bool) && ps.Length > 1 && !(ps[1] is bool))
            return;

        bool isOpen = (bool)ps[0];

        if (isOpen)
        {
            bool isTitle = (ps.Length > 1) ? (bool)ps[1] : false;
            volume.profile = isTitle ? titleData : cutSceneData;
        }

        volume.gameObject.SetActive(isOpen);
    }
}