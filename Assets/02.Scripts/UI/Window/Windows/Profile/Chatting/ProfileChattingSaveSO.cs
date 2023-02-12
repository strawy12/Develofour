using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Profile/Chat/Save")]
public class ProfileChattingSaveSO : ScriptableObject
{
    [Header("��Ͽ�")]
    [SerializeField]
    public List<ChatData> chatDataList;

    [Header("�����")]
    [SerializeField]
    public List<string> saveList;
}
