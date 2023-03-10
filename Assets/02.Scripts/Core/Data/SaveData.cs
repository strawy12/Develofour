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
    // ���� ����� ��� �����Ұų�
    // ���� �����̶�� Ű�� ��ݿ��θ� ������ ����
    // �߿��� �Ŵ� ���� ����� ����Ʈ�� true�̰�, �̸� ��� �Ұų�
    // ������ �ٽ� ������ �� �� �����Ϳ� SO�� �ٸ��� �����͸� �Ͼ����
    // SO�� ������Ʈ �ϸ鼭 �ʱ�ȭ�� �Ǿ��ų�
    // �� ���� ����� Save ���� ��ü�� �츮�� �ʱ�ȭ ��ũ��Ʈ�� �����ϴ°ž�
    // 1. string Format ���
    // 2. ���� �ʱ�ȭ ���

    // 10���޺��� �� ������Ʈ �������ݾ�
    // 10 11 12 1 2 3


    public List<PinLockData> pinLockData;

    public bool isSuccessLoginZoogle;
    public bool isWindowsLoginAdminMode;
    public bool isSuccessLoginStarbook;
}