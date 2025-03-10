using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadybugBabyContainer : MonoBehaviour, IDeathAction
{
    [SerializeField]
    private BaddieVertSine[] baby;

    public void Died()
    {
        for(int i = 0; i < baby.Length; i++) 
        {
            baby[i].gameObject.SetActive(true);
            // Important! we're going to kill the parent, so the babies must be reparented
            // (crikey, that sounds dark).
            baby[i].transform.SetParent(transform.parent);
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
