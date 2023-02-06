using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Profile/Chat/Save")]
public class ProfileChattingSaveSO : ScriptableObject
{
    [SerializeField]
    public List<EProfileChatting> saveList;
}
