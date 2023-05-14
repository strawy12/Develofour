using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "SO/Call/RequestData")]
public class RequestCallDataSO : ScriptableObject
{
    public ECharacterDataType characterType;
    public List<int> monologIDList;
}
