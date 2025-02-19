using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private object pool; // Store as object to handle any ObjectPool<T>

    public void SetPool<T>(ObjectPool<T> pool) where T : MonoBehaviour
    {
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        if (pool is ObjectPool<MonoBehaviour> monoPool)
        {
            monoPool.ReturnToPool(this);
        }
    }
}
