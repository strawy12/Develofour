using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/MediaPlayer/Data")]
public class MediaPlayerDataSO : ScriptableObject
{
    public float textPlaySpeed = 0.05f;
    public AudioClip mediaAudioClip;
    public EProfileCategory category;

    public MediaPlayerBody body;

    [TextArea(10, 20)]
    public string textData;
    public string fileName;

    public float endlineDelay;
    public List<int> infoID;

}
