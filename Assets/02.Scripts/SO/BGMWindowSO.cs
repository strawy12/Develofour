using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/BackgroundBGM")]
public class BGMWindowSO : ScriptableObject
{
    [SerializeField]
    private string id;
    public Sound.EAudioType audioType;

    public string ID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(value))
                return;
            id = value;
        }
    }
}
