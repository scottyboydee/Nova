using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieShipSpitter : MonoBehaviour
{
    [SerializeField]
    RectTransform spriteRect;

    [SerializeField]
    private GameObject[] ChildrenToSpit;

    [SerializeField]
    private float SpitDelayMax = 1f;

    [SerializeField]
    private float SpitOffset = 32;

    private static float spitDelay;

    private const int NUM_CHILDREN = 10;

    private static List<string> Spitters = new List<string>();

    private static int spatSoFar = 0;
    private static int turnNum = 0;

    private int myNum;

    // Start is called before the first frame update
    void Start()
    {
        spitDelay = SpitDelayMax;

        // TODO: this is an awful hack and relies on the spitters having unique names, MUST improve this when I have time.
        int index = Spitters.IndexOf(gameObject.name);
        if ( index < 0)
        {
            Spitters.Add(gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spitDelay > 0)
        {
            spitDelay -= Time.deltaTime;
            return;
        }

        int myNum = Spitters.IndexOf(gameObject.name);

        turnNum++;
        if (turnNum > Spitters.Count)
            turnNum = 0;

        if (turnNum != myNum)
            return;

        UpdateSpitting();

        spitDelay = SpitDelayMax;
    }

    private void UpdateSpitting()
    {
        SpriteTools.Enclose enclosed = SpriteTools.Inst.CheckEnclosure(spriteRect);

        if (enclosed != SpriteTools.Enclose.Inside)
            return;

        int numActiveChildren = CountNonSpitters();

//        Debug.Log("My turn: " + gameObject.name);

        if(numActiveChildren >= NUM_CHILDREN)
        {
//            Debug.Log("Got enough children, not spitting more.");
            return;
        }

//        Debug.Log("Only have " + numActiveChildren + " need to spit out a new one!");

        SpitOutNewChild();
    }

    private void SpitOutNewChild()
    {
        GameObject childToSpit = ChildrenToSpit[spatSoFar % ChildrenToSpit.Length];

        GameObject newChild = Instantiate(childToSpit);
        newChild.transform.SetParent(transform.parent, false);
        newChild.transform.localPosition = transform.localPosition - new Vector3 (0f, SpitOffset, 0f);
//        Debug.Log("SpitterParent position: " + transform.localPosition + " child position: " + newChild.transform.localPosition);
        newChild.name = "SpitterBaby" + spatSoFar;

        newChild.SetActive(true);

        spatSoFar++;

//        Debug.Log("Spat out another baby: " + newChild.name);

    }

    private int CountNonSpitters()
    {
        int total = 0;

        for( int i = 0; i < WaveManager.Instance.Baddies.Count; i++)
        {
            total++;

            GameObject checkAgainst = WaveManager.Instance.Baddies[i].gameObject;

            for ( int j = 0; j < Spitters.Count; j++)
            {
                if(checkAgainst.name == Spitters[j])
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
