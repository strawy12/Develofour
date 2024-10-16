using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BGMWindow : Window
{
    private static bool isChanged;

    [SerializeField]
    private TMP_Text mainText;
    [SerializeField]
    private float playSoundDelay = 0.5f;

    private Sound.EAudioType audioType;

    protected override void Init()
    {
        base.Init();
        audioType = ResourceManager.Inst.GetResource<BGMWindowSO>(file.ID).audioType;
    }

    public override void WindowOpen(bool isNewWindow)
    {
        base.WindowOpen(isNewWindow);

        StartCoroutine(ChangeBGMCoroutine());
    }

    private IEnumerator ChangeBGMCoroutine()
    {
        if (isChanged) yield break;
        isChanged = true;
        yield return new WaitForSeconds(playSoundDelay);
        Sound.OnPlaySound?.Invoke(audioType);
        isChanged = false;
        WindowClose();
    }
}
