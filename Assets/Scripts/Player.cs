using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ICollide
{
    [SerializeField]
    public PlayerGun gun;

    [SerializeField]
    private Transform GunPos;

    [SerializeField]
    private PlayerControls controls;

    [SerializeField]
    private Image playerSprite;

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

        ClampPosition();
    }

    private void ClampPosition()
    {
        float xMin = playerSprite.rectTransform.rect.width / 2;

        if( transform.position.x < xMin )
        {
            transform.position = new Vector3(xMin, transform.position.y, 0);
            controls.Reset();
        }

        float xMax = SpriteTools.Inst.Canvas.pixelRect.width - (playerSprite.rectTransform.rect.width / 2);

        if (transform.position.x > xMax)
        {
            transform.position = new Vector3(xMax, transform.position.y, 0);
            controls.Reset();
        }

    }

    public void Fire()
    {
        gun.Fire( GunPos.position );
    }

    public void Collide()
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Player here - I DIED!");

        GameManager.Instance.PlayerDied();

        WaveManager.Instance.ExplosionManager.AddExplosion(gameObject);

        controls.Reset();
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
