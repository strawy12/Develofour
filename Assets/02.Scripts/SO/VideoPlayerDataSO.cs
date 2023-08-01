using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/VideoPlayer/Data")]
public class VideoPlayerDataSO : ResourceSO
{
    //id is fileID
    public string FileID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(id))
                return;

            id = value;
        }
    }

    public Sprite sprite;
    public CutScene cutScene;
    public Vector2 imageSize;
}
