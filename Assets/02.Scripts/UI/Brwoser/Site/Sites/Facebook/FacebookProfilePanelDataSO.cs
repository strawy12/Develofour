using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Facebook/PidDataSO")]
public class FacebookProfilePanelDataSO : ScriptableObject
{
    public Sprite profileImage;
    public string nameText;
    public string infoText;

    public List<FacebookPidPanel> pidList;
}
