using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/OutStar/CharacterData")]
public class OutStarProfileDataSO : ResourceSO
{
    public string ID { get => id; }
    public string SetID { set => id = value; }

    public List<string> timeChatIDList;
}
