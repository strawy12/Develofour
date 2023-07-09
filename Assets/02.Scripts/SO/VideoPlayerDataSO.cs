using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/VideoPlayer/Data")]
public class VideoPlayerDataSO : ScriptableObject
{
    private string fileId;

    public string FileID
    {
        get => fileId;
        set
        {
            if (!string.IsNullOrEmpty(fileId))
                return;

            fileId = value;
        }
    }

    public Sprite sprite;
    public CutScene cutScene;
    public Vector2 imageSize;
}
