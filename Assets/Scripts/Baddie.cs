using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour, IExplode, ICollide
{
    [SerializeField]
    AnimSprite myAnimSprite;

    public void Explode()
    {
        WaveManager.Instance.ExplosionManager.AddExplosion(gameObject);
    }

    private void Die()
    {
//        Debug.Log("Baddie Die: " + gameObject.name);
        WaveManager.Instance.RemoveBaddieFromList(this);
        Destroy(gameObject);
    }

    public void Collide()
    {
 //       Debug.Log("Collide: I'm a baddie and I just got hit! Name: " + gameObject.name);
        Explode();
        Die();
    }

    // Start is called before the first frame update
    void Start()
    {
        WaveManager.Instance.AddBaddieToList(this);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
