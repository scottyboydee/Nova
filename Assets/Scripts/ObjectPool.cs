using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> pool = new Queue<T>();

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

        Debug.Log("Objects left in " + prefab.name + " pool: " + pool.Count);
        return obj;
    }

    private T Instantiate()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.name += "(" + (instNum++) + ")";

        PooledObject pooled = obj.gameObject.AddComponent<PooledObject>();
        pooled.SetPool(this); // Pass the pool to the non-generic component

        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent); // Ensure it stays under the parent
        pool.Enqueue(obj);

        Debug.Log("Object returned to " + prefab.name + " pool. Now have: " + pool.Count);
    }

}
