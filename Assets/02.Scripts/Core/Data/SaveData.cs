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
    // ���� ����� ��� �����Ұų�
    // ���� �����̶�� Ű�� ��ݿ��θ� ������ ����
    // �߿��� �Ŵ� ���� ����� ����Ʈ�� true�̰�, �̸� ��� �Ұų�
    // ������ �ٽ� ������ �� �� �����Ϳ� SO�� �ٸ��� �����͸� �Ͼ����
    // SO�� ������Ʈ �ϸ鼭 �ʱ�ȭ�� �Ǿ��ų�
    // �� ���� ����� Save ���� ��ü�� �츮�� �ʱ�ȭ ��ũ��Ʈ�� �����ϴ°ž�
    // 1. string Format ���
    // 2. ���� �ʱ�ȭ ���

    // fileSO isLock �̰Ŵ� ���, ��� 
    // ���� ������
    // fileSO Ž�� isLock true�� �ֵ��� ����ٰ� �־��ִ� �ݺ����� ����
    // Ŭ���ϸ� ����Ǵ� ������ �Լ��� Json �������� 
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // {"/file/fileA", true },
    // Log���ٰ� �����
    // �� �̾��� �����͸� �����ؼ� �ٿ��ֱ� ���ش�

    // Json string���� ������
    // �ڵ带 �ۼ��� �Ұ���
    // �ٽ� �ѹ� �����
    // string  1
    // �ڵ�    1
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
