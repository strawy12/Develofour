using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ESaveDataType
{
    None = -1,
    PinLockData,

    ComputerLoginState,

    IsSuccessLoginZoogle,
    IsSuccessLoginStarbook,
    IsWatchStartCutScene,
}

[System.Serializable]
public class PinLockData
{
    public string fileLocation;
    public bool isLock = true;
}
[System.Serializable]
public class MonologSaveData
{
    public ETextDataType monologType;
    public bool isShow;
}
[System.Serializable]
public class SaveData
{
    // 파일 잠금은 어떻게 저장할거냐
    // 파일 네임이라는 키와 잠금여부를 가지는 벨류
    // 중요한 거는 파일 잠금은 디폴트가 true이고, 이를 어떻게 할거냐
    // 게임을 다시 실행할 때 이 데이터와 SO가 다르면 데이터를 믿어야해
    // SO를 업데이트 하면서 초기화가 되었거나
    // 뭐 좋은 방법은 Save 파일 자체를 우리가 초기화 스크립트를 제공하는거야
    // 1. string Format 방식
    // 2. 변수 초기화 방식

    // fileSO isLock 이거는 상수, 어떻게 
    // 툴을 만들자
    // fileSO 탐색 isLock true인 애들을 여기다가 넣어주는 반복문을 하자
    // 클릭하면 실행되는 에디터 함수들 Json 형식으로 
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // Log에다가 찍어줘
    // 이 뽑아준 데이터를 복사해서 붙여넣기 해준다

    // Json string으로 쓸건지
    // 코드를 작성을 할건지
    // 다시 한번 물어보고
    // string  1
    // 코드    1
    public List<PinLockData> pinLockData;
    public List<MonologSaveData> monologSaveData;
    public bool isSuccessLoginZoogle;
    public bool isSuccessLoginStarbook;
    public bool isWatchStartCutScene;
    
    public EComputerLoginState computerLoginState;
}

public enum EComputerLoginState
{
    Logout,
    Guest,
    Admin,
}
