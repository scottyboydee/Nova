using UnityEngine;

public class PooledObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private ObjectPool<T> pool;

    public void SetPool(ObjectPool<T> pool)
    {
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        pool?.ReturnToPool(this as T);
    }
}
