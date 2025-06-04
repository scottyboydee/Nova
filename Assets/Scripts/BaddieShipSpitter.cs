using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieShipSpitter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ChildrenToSpit;

    private static List<GameObject> Spitters = new List<GameObject>();

    private static int spatSoFar = 0;
    private static int spitTypeIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Spitters.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpitting();
    }

    private void UpdateSpitting()
    {
        int numActiveChildren = CountNonSpitters();
    }

    private int CountNonSpitters()
    {
        int total = 0;

        for( int i = 0; i < WaveManager.Instance.Baddies.Count; i++)
        {
            total++;

            for( int j = 0; j < Spitters.Count; j++)
            {
                if(WaveManager.Instance.Baddies[i] == Spitters[j])
                {
                    total--;
                    break;
                }
            }
        }

        Debug.Log("Got " + total + " non-spitter baddies");

        return total;
    }
}
