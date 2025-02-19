using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolReturnable
{
    void ReturnToPool(Object obj);
}
