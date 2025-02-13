using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, INotify
{
    [SerializeField]
    AnimSprite mySprite;

    private ExplosionManager manager;

    public void SetManager(ExplosionManager manager)
    { 
        this.manager = manager;
    }

    public void Notify(NotifyType notification)
    {
        if( notification != NotifyType.AnimFinished )
        {
            return;
        }

        Debug.Log("Anim finished!");
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if( manager != null ) 
        {
            Debug.Log("Explosion:ReturnToPool");
            manager.ExplosionFinished(this);
        }
        else
        {
            Debug.Log("No manager to inform of finished explosion");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
