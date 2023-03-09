using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Profile/Chat/Save")]
public class ProfileChattingSaveSO : ScriptableObject
{
    [Header("기록용")]
    [SerializeField]
    public List<ChatData> chatDataList;

    [Header("저장용")]
    [SerializeField]
    public List<string> saveList;
}
