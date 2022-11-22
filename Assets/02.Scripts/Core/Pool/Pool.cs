using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour, IPoolable
{
    private Stack<T> pool;
    private Transform parent;
    private T prefab;

    public Pool(T prefab, Transform parent, int amount)
    {
        pool = new Stack<T>();
        this.prefab = prefab;
        this.parent = parent;

        for(int i = 0; i < amount; i++)
        {
            T temp = GameObject.Instantiate(prefab, parent);
            pool.Push(temp);
        }
    }

    public void Push(T obj)
    {
        if(obj == null) { return; }
        obj.Reset();
        pool.Push(obj);
    }

    public T Pop()
    {
        T obj = null;
        if(pool.Count == 0)
        {
            obj = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            obj = pool.Pop();
        }

        if(obj != null && obj.gameObject.activeSelf == false)
        {
            obj.gameObject.SetActive(true);
        }

        return obj;
    }

    public void Clear()
    {
        while(pool.Count > 0)
        {
            T temp = pool.Pop();
            temp.Reset();
            GameObject.Destroy(temp.gameObject);
        }
        pool.Clear();
    }
}
