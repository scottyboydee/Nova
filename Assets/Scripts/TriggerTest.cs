using UnityEngine;


public class TriggerTest : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("TriggerTest Awake");



    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered! " + other.gameObject.name + " entered " + gameObject.name);
    }
}

