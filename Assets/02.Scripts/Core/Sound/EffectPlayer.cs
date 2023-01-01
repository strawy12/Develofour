using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : SoundPlayer
{
    [SerializeField]
    private Sound.EEffect effectSoundID;


    public override void SetValue(AudioClip clip)
    {
        base.SetValue(clip);
        effectSoundID = Sound.EEffect.Count;
    }
    public override void Init()
    {
        currentPlayerType = ESoundPlayerType.Effect;
        soundID = (int)effectSoundID;
        base.Init();
    }

    private void Reset()
    {
        effectSoundID = Sound.EEffect.None;
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if (effectSoundID != (Sound.EEffect)transform.GetSiblingIndex())
        {
            effectSoundID = (Sound.EEffect)transform.GetSiblingIndex();
        }
    }
}
