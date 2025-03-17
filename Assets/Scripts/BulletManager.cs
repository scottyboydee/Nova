using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private PlayerShot baddieBulletPrefab;

    [SerializeField]
    private GameObject bulletsParent;

    private ObjectPool<PlayerShot> baddieBulletPool;

    internal void AddBaddieShot(GameObject anchor, float heatSeek = 0.0f)
    {
        if (anchor == null)
        {
            Debug.Log("EEK! AddItem: no anchor!");
            return;
        }

//        Debug.Log("AddBaddieShot from: " + anchor.name);

        PlayerShot newItem = baddieBulletPool.Get();
        newItem.SetHeatSeek(heatSeek);
        newItem.transform.position = anchor.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        baddieBulletPool = new ObjectPool<PlayerShot>(baddieBulletPrefab, 20, bulletsParent.transform);
    }

    public void CleanUpAllBullets()
    {
        Debug.Log("CleanUpAllBullets");
        baddieBulletPool.ReturnAllToPool();
    }


}
