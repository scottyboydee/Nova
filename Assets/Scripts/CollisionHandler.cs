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
//            Debug.Log("I'm a PlayerShot! We must have hit a baddie!");
            ICollide collideRecipient = other.transform.parent.GetComponent<ICollide>();
            if(collideRecipient == null )
            {
                Debug.Log("EEK! CollisionHandler: PlayerShot didn't find collideRecipient!");
                return;
            }

//            Debug.Log("PlayerShot found a collideRecipient to inform!");
            collideRecipient.Collide();
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

