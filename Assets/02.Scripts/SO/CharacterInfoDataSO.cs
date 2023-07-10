using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/CharacterInfoDataSO")]
public class CharacterInfoDataSO : ScriptableObject 
{
    public string id;
    public string characterName;
    public string phoneNum;
    public Sprite profileIcon;
    public ECharacterDataType characterType;
}
