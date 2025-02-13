using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField]
    private Explosion explosionPrefab;

    [SerializeField]
    private GameObject explosionParent;

    private ObjectPool<Explosion> explosionPool;

    internal void AddExplosion(GameObject anchor)
    {
        if( anchor == null )
        {
            Debug.Log("EEK! AddExplosion: no anchor!");
            return;
        }

//        Debug.Log("AddExplosion from: " + anchor.name);

        Explosion newExplosion = explosionPool.Get();
        newExplosion.SetManager(this);
        newExplosion.transform.position = anchor.transform.position;
    }

    public void ExplosionFinished(Explosion explosion)
    {
//        Debug.Log("Returning to pool: "+ explosion.name);
        explosionPool.ReturnToPool(explosion);
    }

    // Start is called before the first frame update
    void Start()
    {
        explosionPool = new ObjectPool<Explosion>(explosionPrefab, 20, explosionParent.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
