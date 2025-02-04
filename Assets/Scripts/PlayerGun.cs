using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    GameObject sourceShot;

    static readonly int MAX_SHOTS = 10;

    PlayerShot[] shots = new PlayerShot[MAX_SHOTS];

    // Start is called before the first frame update
    void Start()
    {
        sourceShot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private PlayerShot GetFreeShot()
    {
        PlayerShot useShot = null;
        int useShotNum = -1;
        int activeShots = 0;

        for( int i = 0; i < shots.Length; i++)
        {
            if (shots[i] && shots[i].isActiveAndEnabled )
            {
                activeShots++;
                continue;
            }

            useShotNum = i;
            break;
        }

//        Debug.Log("Active shots: " + activeShots + " out of " + MAX_SHOTS);

        if (useShotNum < 0)
        {
            Debug.Log("Couldn't get a shot to use! Ran out!");
            return null;
        }

        useShot = shots[useShotNum];

        if (useShot == null)
        { 
            GameObject newShot = sourceShot.Clone(sourceShot.transform.parent, sourceShot.gameObject.name + "_" + useShotNum);
            useShot = newShot.GetComponent<PlayerShot>();
            shots[useShotNum] = useShot;

            Debug.Log("Created new shot, now have " + useShotNum + "/" + MAX_SHOTS);
        }

        useShot.gameObject.SetActive(true);

        return useShot;
    }

    public void Fire( Vector3 playerPos )
    {
        PlayerShot shot = GetFreeShot();

        if( shot == null )
        {
            Debug.Log("No shot available!");
            return;
        }

        shot.transform.position = playerPos;
    }
}
