using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour, IExplode, ICollide
{
    [SerializeField]
    private int MaxLives = 1;

    [SerializeField]
    private GameObject notifyWhenDead;
    private IDeathAction notifyTarget;

    private int numLives;

    [SerializeField]
    private ScoreValue Value;

    public enum ScoreValue
    {
        Small,
        Medium,
        Large,
        Boss
    }


    void Awake()
    {
        if (notifyWhenDead != null)
        {
            notifyTarget = notifyWhenDead.GetComponent<IDeathAction>();
            if (notifyTarget == null)
            {
                Debug.LogError($"{notifyWhenDead.name} does not implement IDeathAction!");
            }
        }

        WaveManager.Instance.AddBaddieToList(this);
    }

    public void Explode()
    {
        WaveManager.Instance.ExplosionManager.AddExplosion(gameObject);
    }

    private void Die()
    {
        //        Debug.Log("Baddie Die: " + gameObject.name);
        notifyTarget?.Died();
        
        ScoreManager.Instance.AddScoreByValue(Value);

        removeFromWave();
    }

    private void removeFromWave()
    {
        WaveManager.Instance.RemoveBaddieFromList(this);
        Destroy(gameObject);
    }

    // gosh, this sounds very authoritarian...
    public void RemoveYourself()
    {
        removeFromWave();
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
//        WaveManager.Instance.AddBaddieToList(this);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
