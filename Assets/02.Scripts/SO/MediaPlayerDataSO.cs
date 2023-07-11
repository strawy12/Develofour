using DG.Tweening.Plugins;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/MediaPlayer/Data")]
public class MediaPlayerDataSO : ResourceSO
{
    //id is fileID
    public float textPlaySpeed = 0.05f;
    public AudioClip mediaAudioClip;

    public MediaPlayerBody body;
     
    [TextArea(10, 20)]
    public string textData;

    public float endlineDelay;
    public List<int> infoID;

    [ContextMenu("SetScripts")]
    public void SetScripts()
    {
        if (body != null)
        {
            textData = body.GetComponentInChildren<TMPro.TMP_Text>().text;
        }
    }

}
