using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpitter : MonoBehaviour
{
    [SerializeField]
    private float Timer;

    [SerializeField]
    private float Probability = 1.0f;

    [SerializeField]
    private float heatSeek = 0.0f;

    private float timeLeft;



    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        timeLeft = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        if( timeLeft < 0 )
        {
            ResetTimer();

            float random = Random.value;
            if( random <= Probability )
            {
                Fire();
            }
            else
            {
//                Debug.Log("Not firing, due to random " + random + " > probability: " + Probability);
            }
        }
    }

    private void Fire()
    {
//        Debug.Log("FIRE!");
        WaveManager.Instance.BulletManager.AddBaddieShot(gameObject, heatSeek);
    }
}
