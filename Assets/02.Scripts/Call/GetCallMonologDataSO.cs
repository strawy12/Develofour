using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GetCallMonolog
{
    public ECharacterDataType charData;
    public int textData;
}

[CreateAssetMenu(menuName = "SO/Call/GetMonolog")]
public class GetCallMonologDataSO : ScriptableObject
{
    public List<GetCallMonolog> GetCallMonologList = new List<GetCallMonolog>();
}
