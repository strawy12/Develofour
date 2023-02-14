using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;

public class Test : ScriptableObject
{
    object so;
    void Start()
    {

        Type soType = Type.GetType("TestSO1");
        object obj = CreateInstance(soType);
        obj as SOParent;
    }

}

