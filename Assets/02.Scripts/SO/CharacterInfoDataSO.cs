using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterType
{
    None,
    KangYohan,
    ParkJuyoung,
    KimYujin,
    Assistant,
    Police,
    Security,
    HanTaewoong,
}


[CreateAssetMenu(menuName = "SO/CharacterInfoDataSO")]
public class CharacterInfoDataSO : ResourceSO 
{
    public string characterName;
    public string phoneNum;
    public string rollText;
    public Sprite profileIcon;
    public ECharacterType type;

    public string ID
    {
        get => id;
        set
        {
            if (!string.IsNullOrEmpty(id))
                return;

            id = value;
        }
    }
}
