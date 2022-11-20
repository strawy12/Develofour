using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class A : MonoBehaviour, IPoolable
{
    public void Reset()
    {
    }
}

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<Type, object> pools = new Dictionary<Type, object>();

    public void CreatePool<T>(T prefab, Transform parent, int count = 10) where T : MonoBehaviour, IPoolable
    {
        Pool<T> pool = new Pool<T>(prefab, parent);
        pools.Add(typeof(T), pool);
    }

    public void Push<T>(T obj) where T : MonoBehaviour, IPoolable
    {
        Pool<T> pool = pools[obj.GetType()] as Pool<T>;
        pool.Push(obj);
    }

    public T Pop<T>() where T : MonoBehaviour, IPoolable
    {
        if (!pools.ContainsKey(typeof(T)))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            return null;
        }

        Pool<T> pool = pools[typeof(T)] as Pool<T>;
        return pool.Pop();
    }


    private void OnDestroy()
    {
        foreach(object pool in pools.Values)
        {
            System.Reflection.MethodInfo methodInfo = pool.GetType().GetMethod("Clear");

            methodInfo?.Invoke(pool, null);
        }
    }
}
