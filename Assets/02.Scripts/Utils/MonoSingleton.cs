using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _inst = null;
    private static bool shuttingDown = false;
    private static object locker = new object();

    public static T Inst
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance " + typeof(T) + " already destroyed. Returning null.");
            }

            lock (locker)
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

    private void OnDestroy()
    {
        shuttingDown = true;
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

}
