using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie : MonoBehaviour, IExplode
{
    [SerializeField]
    AnimSprite myAnimSprite;

    public void Explode()
    {
        Debug.Log("Boom! The bomb exploded.");
        if( myAnimSprite == null )
        {
            Debug.Log("No explosion anim to fire for: " + gameObject.name);
            return;
        }
        myAnimSprite.gameObject.SetActive(true);
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
