using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    [SerializeField]
    private PlayerData playerData;
    public PlayerData CurrentPlayer => playerData;

    private string SAVE_PATH = "";
    private const string SAVE_FILE = "";

    private void Awake()
    {
        // SAVE_PATH = Application.dataPath

    }

    private void CheckDirectory()
    {
        if(/*�ش� Path�� Directory�� ���ٸ�*/)
        {
            // Directory ����
        }
    }

    private void CreatePlayerData()
    {
        playerData = new PlayerData();
    }

    private void LoadFromJson()
    {
        if (/*�ش� PATH�� �ش� ������ �����Ѵٸ�*/)
        {
            // �ش� Json�� �ҷ��ͼ� PlayerData Ŭ���� �������� ��ȯ ��Ű��
        }
        else
        {
            CreatePlayerData();
        }

        SaveToJson();
    }

    private void SaveToJson()
    {
        // playerData�� json ������ string���� �ٲ��ְ� ���Ͽ��ٰ� �� ��Ʈ�� ���� ���ش�
    }

    private void OnDestroy()
    {
        SaveToJson();
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
