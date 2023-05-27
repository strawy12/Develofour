using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "SO/Call/IncomingData")]
public class IncomingCallDataSO : ScriptableObject
{
    public ECharacterDataType characterType;
    public List<ReturnMonologData> incomingMonologList;
}
