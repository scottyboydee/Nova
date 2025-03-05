using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour, ICollide
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private RectTransform rectTransform;

    private PooledObject pooledObject;

    public void Fire( Vector3 fromPos )
    {
        transform.position = fromPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float yMove = speed * Time.deltaTime;
        Vector3 moveBy = new Vector3(0, yMove, 0);
        transform.position += moveBy;

        SpriteTools.Enclose enclosed = SpriteTools.Inst.CheckEnclosure(rectTransform);
 //       Debug.Log("Bullet: " + gameObject.name + " enclosure: " + enclosed);
        if (enclosed == SpriteTools.Enclose.Outside)
            Deactivate();
    }

    private void Deactivate()
    {
        if(pooledObject != null)
        {
            pooledObject.ReturnToPool();
            return;
        }

        gameObject.SetActive(false);
    }

    public void Collide()
    {
        Deactivate();
    }
}
