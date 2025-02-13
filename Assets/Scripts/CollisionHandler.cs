using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    void Awake()
    {
//        Debug.Log("CollisionHandler Awake for " + gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision! " + other.gameObject.name + " entered " + gameObject.name);

        if( gameObject.layer == LayerNames.PlayerShot)
        {
            Debug.Log("I'm a PlayerShot! We must have hit a baddie!");
            Baddie baddie = other.gameObject.GetComponent<Baddie>();
            if( baddie == null )
            {
                Debug.Log("EEK! CollisionHandler: PlayerShot didn't find baddie!");
                return;
            }

            Debug.Log("PlayerShot found a baddie to kill!");
        }

        /*
            IExplode explosive = gameObject.GetComponent<IExplode>();
            if (explosive != null)
            {
                explosive.Explode();
            }

            // HACK! TOTAL AND UTTER HACK!!
            explosive = other.gameObject.transform.parent.parent.gameObject.GetComponent<IExplode>();
            if (explosive != null)
            {
                explosive.Explode();
            }
        */
    }
}

