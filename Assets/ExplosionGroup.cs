using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class ExplosionGroup : MonoBehaviour
{
    const int TIMER_DONE = -1;

    [SerializeField]
    private AnimSprite[] explosions;

    [SerializeField]
    private float[] delay;

    private float[] timer;

    // Start is called before the first frame update
    void Start()
    {
        if( explosions == null || explosions.Length == 0 )
        {
            Debug.LogError("Explosion Group - has no explosions! Name:" + gameObject.name);
            return;
        }

        if (delay == null || delay.Length < explosions.Length)
        {
            Debug.LogError("Explosion Group - wrong number of delays: " + delay.Length + " vs " + explosions.Length + "! Name:" + gameObject.name);
            return;
        }

        ResetTimers();

        gameObject.SetActive(false);
    }

    private void ResetTimers()
    {
        timer = new float[delay.Length];
        for (int i = 0; i < delay.Length; i++)
        {
            timer[i] = delay[i];
            explosions[i].gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        ResetTimers();
        for( int i = 0; i < explosions.Length; i++ )
        {
            explosions[i].Reset();
            explosions[i].gameObject.SetActive(false);
        }
    }

    public void Explode(Vector3 matchPos)
    {
        Reset();

        if(matchPos != null)
        {
            transform.position = matchPos;
        }

        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < explosions.Length; i++ ) 
        {
            // already done, so skip
            if (timer[i] == TIMER_DONE)
                continue;

            timer[i] -= Time.deltaTime;

            if (timer[i] > 0)
                continue;

            Debug.Log("ExplosionGroup " + gameObject.name + " firing explosion: " + explosions[i].name + " after time: " + delay[i]);

            explosions[i].gameObject.SetActive(true);

            timer[i] = TIMER_DONE;
        }
        
    }
}
