using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/VideoPlayer/Data")]
public class VideoPlayerDataSO : ScriptableObject
{
    public string fileName;
    public Sprite sprite;
    public CutScene cutScene;
}
    