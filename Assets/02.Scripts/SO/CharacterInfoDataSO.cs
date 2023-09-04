using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/CharacterInfoDataSO")]
public class CharacterInfoDataSO : ResourceSO 
{
    public string characterName;
    public string phoneNum;
    public string rollText;
    public Sprite profileIcon;

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
