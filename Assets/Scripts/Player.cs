using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public PlayerGun gun;

    [SerializeField]
    private Transform GunPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer( float dx )
    {
        float xMove = dx * Time.deltaTime;
        Vector3 moveBy = new Vector3(xMove, 0, 0);

        gameObject.transform.position += moveBy;

    }

    public void Fire()
    {
        gun.Fire( GunPos.position );
    }
}

public static class GameObjectExtensions
{
    public static GameObject Clone(
        this GameObject source,
        Transform parent = null,
        string newName = null,
        Vector3? newPosition = null)
    {
        // Clone the object
        GameObject clone = Object.Instantiate(source);

        // Set the parent if provided
        if (parent != null)
        {
            clone.transform.SetParent(parent);
        }

        // Set the name
        clone.name = newName ?? (source.name + "_Clone");

        // Set the position
        clone.transform.position = newPosition ?? source.transform.position;

        return clone;
    }
}
