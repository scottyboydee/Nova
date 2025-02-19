using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject baddieBulletPrefab;

    [SerializeField]
    private GameObject bulletsParent;

//    private ObjectPool<Explosion> baddieBulletPool;

    internal void AddItem(GameObject anchor)
    {
        if (anchor == null)
        {
            Debug.Log("EEK! AddItem: no anchor!");
            return;
        }

        Debug.Log("AddItem from: " + anchor.name);

//        Transform newItem = baddieBulletPool.Get();
     //   newItem.position = anchor.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
//        baddieBulletPool = new ObjectPool<Transform>(baddieBulletPrefab.transform, 20, bulletsParent.transform);
    }


}
