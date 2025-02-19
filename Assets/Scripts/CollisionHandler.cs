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

        if (gameObject.layer == LayerNames.BaddieShot)
        {
            Debug.Log("I'm a BaddieShot! We must have hit the player!");

            ICollide collideRecipient = other.transform.parent.GetComponent<ICollide>();
            if (collideRecipient == null)
            {
                Debug.Log("EEK! CollisionHandler: BaddieShot didn't find collideRecipient!");
                return;
            }

//            Debug.Log("BaddieShot found a collideRecipient to inform!");
            collideRecipient.Collide();

        }

    }
}

