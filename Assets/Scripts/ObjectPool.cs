using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

public class ObjectPool<T> : IPoolReturnable where T : MonoBehaviour
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private int instNum = 0;

    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Instantiate();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T Get(bool active = true)
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            Debug.Log("EEK: " + prefab.name + " POOL RAN OUT, INSTANTIATING!");
            obj = Instantiate();
        }

        obj.gameObject.SetActive(active);
        return obj;
    }

    private T Instantiate()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.name += "(" + (instNum++) + ")";

        PooledObject pooled = obj.gameObject.GetComponent<PooledObject>() ?? obj.gameObject.AddComponent<PooledObject>();
        pooled.SetPool(this, obj); // Pass the pool as an IObjectPool

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        pool.Enqueue(obj);
    }

    void IPoolReturnable.ReturnToPool(Object obj)
    {
        T casted = obj as T;
        ReturnToPool(casted);
    }
}
