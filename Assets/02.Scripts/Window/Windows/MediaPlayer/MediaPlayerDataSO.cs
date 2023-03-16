using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/MediaPlayer/Data")]
public class MediaPlayerDataSO : ScriptableObject
{
    public float textPlaySpeed = 0.05f;
    public string[] textData;
    public AudioClip mediaAudioClip;
}
