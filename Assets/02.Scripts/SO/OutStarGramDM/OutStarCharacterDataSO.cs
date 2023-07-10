using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/OutStar/CharacterData")]
public class OutStarCharacterDataSO : ScriptableObject
{
    [SerializeField]
    private string characterId;

    public string ID { get => characterId; }

    public string SetID { set => characterId = value; }

    public List<string> timeChatIDList;
}
