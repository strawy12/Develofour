using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EvidenceType")]
public class EvidenceTypeSO : ResourceSO
{
    public int maxCount; // ��� Ʋ������ ������ �˷��ٰǰ�  0 == �Ⱦ˷���
    public string selectMonolog; // ���������� ������ ����
    public string wrongMonolog; // Ʋ������ ������ ����
    public string wrongHintMonolog; //Ʋ�Ȱ� Maxcount�� á���� ������ ����
    public string answerInfoID;
}
