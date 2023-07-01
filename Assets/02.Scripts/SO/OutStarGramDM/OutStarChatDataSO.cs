using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/OutStar/ChatData")]
public class OutStarChatDataSO : ScriptableObject
{
    private string id;
    public string ID { get => id; }
    public string SetID { set => id = value; }

    public bool isMine;
    public string chatText;
    public string triggerID;
    public int startIdx;
    public int endIdx;
}
