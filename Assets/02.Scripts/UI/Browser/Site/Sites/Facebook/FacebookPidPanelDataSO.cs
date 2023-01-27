using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Site/Facebook/PidDataSO")]
public class FacebookPidPanelDataSO : ScriptableObject
{
    public Sprite profileImage;
    public string profileNameText;
    public string profileTimeText;

    public string pidText;
    public Sprite pidImage;

    public int likeCount;
    public int commentCount;

    [SerializeField]
    public List<FacebookPidCommentData> commentList;
}
