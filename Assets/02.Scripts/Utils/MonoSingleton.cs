using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _inst = null;
    public static T Inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindObjectOfType<T>();
                if (_inst == null)
                {
                    _inst = new GameObject(typeof(T).ToString()).AddComponent<T>();

                }
            }
            return _inst;
        }
    }
}
