using Coffee.UIEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void SaveBGMSoundValue(float value)
    {
        defaultSaveData.BGMSoundValue = value;
    }

    public void SaveEffectSoundValue(float value)
    {
        defaultSaveData.EffectSoundValue = value;
    }
}
