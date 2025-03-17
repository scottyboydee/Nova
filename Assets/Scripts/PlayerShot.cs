using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShot : MonoBehaviour, ICollide
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private RectTransform rectTransform;

    private PooledObject pooledObject;

    private float heatSeek;

    public void SetHeatSeek(float amount)
    {
        heatSeek = amount;
    }

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
        MoveHeatSeek();
    }

    private void MoveHeatSeek()
    {
        float useHeatSeek = heatSeek * Time.deltaTime;

        if (useHeatSeek <= 0)
            return;

        float dx = GameManager.Instance.Player.transform.position.x - transform.position.x;

        float moveX = Mathf.Clamp(dx, -useHeatSeek, useHeatSeek);

        transform.position += new Vector3(moveX, 0, 0);
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
