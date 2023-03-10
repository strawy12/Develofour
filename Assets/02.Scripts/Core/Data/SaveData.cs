using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum ESaveDataType
{
    None = -1,
    PinLockData,
    IsSuccessLoginZoogle,
    IsWindowsLoginAdminMode,
    IsSuccessLoginStarbook,
}

[System.Serializable]
public class PinLockData
{
    public string fileLocation;
    public bool isLock = true;
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

    // 10월달부터 이 프로젝트 진행했잖아
    // 10 11 12 1 2 3


    public List<PinLockData> pinLockData;

    public bool isSuccessLoginZoogle;
    public bool isWindowsLoginAdminMode;
    public bool isSuccessLoginStarbook;
}