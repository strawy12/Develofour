using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/BackgroundBGM")]
public class BGMWindowSO : ScriptableObject
{
    public int id;
    public Sound.EAudioType audioType;
}
