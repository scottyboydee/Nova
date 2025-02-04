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
    }
}

