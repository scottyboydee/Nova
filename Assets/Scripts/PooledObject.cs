using UnityEngine.Pool;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private IPoolReturnable pool; // Store as non-generic interface

    private Object pooledObject;

    public void SetPool(IPoolReturnable pool, Object obj)
    {
        this.pool = pool;
        pooledObject = obj;
    }

    public void ReturnToPool()
    {
        pool?.ReturnToPool(pooledObject);
    }
}
