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
        if(/*해당 Path에 Directory가 없다면*/)
        {
            // Directory 생성
        }
    }

    private void CreatePlayerData()
    {
        playerData = new PlayerData();
    }

    private void LoadFromJson()
    {
        if (/*해당 PATH에 해당 파일이 존재한다면*/)
        {
            // 해당 Json를 불러와서 PlayerData 클래스 형식으로 변환 시키기
        }
        else
        {
            CreatePlayerData();
        }

        SaveToJson();
    }

    private void SaveToJson()
    {
        // playerData를 json 형식의 string으로 바꿔주고 파일에다가 이 스트링 값을 써준다
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
