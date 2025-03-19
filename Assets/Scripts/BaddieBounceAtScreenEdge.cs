using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaddieBounceAtScreenEdge : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private RectTransform myTransform;

    private Rect bounds;

    [SerializeField]
    private Direction8Way direction = Direction8Way.Down;

    [SerializeField]
    private bool randomiseDirection = false;

    [SerializeField]
    private Direction8Way[] randomDirections;

    private Vector3 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        SetupBounds();
        SetupDirection();
    }

    private void SetupBounds()
    {
        bounds = SpriteTools.Inst.Canvas.pixelRect;
        bounds = bounds.PadRectBySpriteRect(myTransform.rect);
    }

    private void SetupDirection()
    {
        if (randomiseDirection)
        {
            int randomIndex = Random.Range(0, randomDirections.Length);
            direction = randomDirections[randomIndex];
        }

        moveDir = SpriteTools.Direction8WayToVector3[(int)direction];
    }

    // Update is called once per frame
    void Update()
    {
        MoveAndCollide();
    }

    private void MoveAndCollide()
    {
        if(moveDir == Vector3.zero)
        {
            Debug.LogError("BaddieBounceAtScreenEdge: movement vector is zero");
            return;
        }

        float useSpeed = speed * Time.deltaTime;

        Vector3 delta = useSpeed * moveDir;

        transform.position += delta;

        // Note: it's important that we consider the direction we're moving, not simply the screen edge
        // that we exceeded, this permits things to move from OFFSCREEN to onscreen, THEN bounce after.
        if(moveDir.y > 0)
        {
            if (transform.position.y > bounds.yMax)
            {
                transform.position = new Vector3(transform.position.x, bounds.yMax, 0);
                moveDir.y *= -1;
            }
        } 
        else if (moveDir.y < 0)
        {
            if (transform.position.y < bounds.yMin)
            {
                transform.position = new Vector3(transform.position.x, bounds.yMin, 0);
                moveDir.y *= -1;
            }
        }

        if(moveDir.x > 0)
        {
            if (transform.position.x > bounds.xMax)
            {
                transform.position = new Vector3(bounds.xMax, transform.position.y, 0);
                moveDir.x *= -1;
            }
        }
        else if (transform.position.x < bounds.xMin)
        {
            transform.position = new Vector3(bounds.xMin, transform.position.y, 0);
            moveDir.x *= -1;
        }
    }
}
