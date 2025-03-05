using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour, IExplode, ICollide
{
    [SerializeField]
    AnimSprite myAnimSprite;

    [SerializeField]
    int MaxLives = 1;

    int numLives;

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

    private void LoseLife()
    {
        numLives--;

        if (numLives > 0)
        {
            Debug.Log("Hit, but numLives: " + numLives + " > 0");
            return;
        }

        Explode();
        Die();
    }

    public void Collide()
    {
        LoseLife();
    }

    // Start is called before the first frame update
    void Start()
    {
        numLives = MaxLives;
        WaveManager.Instance.AddBaddieToList(this);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
