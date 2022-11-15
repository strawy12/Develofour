using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField]
    private PlayerData playerData;
    public PlayerData Data => playerData;

    private void Awake()
    {
        CreatePlayerData();
    }

    private void CreatePlayerData()
    {
        playerData = new PlayerData();
    }
}
