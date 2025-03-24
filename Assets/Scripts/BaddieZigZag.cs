using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieZigZag : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float moveDistance = 32f;

    [SerializeField]
    private LeftRight startDirection = LeftRight.Left;

    [SerializeField]
    private float delayStart;

    [SerializeField]
    private RectTransform spriteRect;

    [SerializeField]
    private Baddie myBaddie;

    private Direction direction;

    private Vector3 nextPos;

    private float delayTimer;

    // Start is called before the first frame update
    void Start()
    {
        delayTimer = delayStart;

        if( startDirection == LeftRight.Left ) 
        {
            direction = Direction.Left;
        }
        else
        {
            direction = Direction.Right;
        }
        CalculateNextPosition();
    }

    private void CalculateNextPosition()
    {
        nextPos = transform.position;

        int dirIndex = (int)direction;

        Vector3 dirVector = SpriteTools.DirectionToVector3[dirIndex];

        nextPos += dirVector * moveDistance;
    }
    private void NextMoveDirection()
    {
        if (startDirection == LeftRight.Left) // we're a down-left zigzagger
        {
            if (direction == Direction.Left)
            {
                direction = Direction.Down; // was going left so now must go down
            }
            else
            {
                direction = Direction.Left; // was doing down so now must go left
            }
        }
        else // were's a down-right zigzagger
        {
            if (direction == Direction.Right)
            {
                direction = Direction.Down; // was going right so now must go down
            }
            else
            {
                direction = Direction.Right; // was doing down so now must go right
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer -= Time.deltaTime;

        if (delayTimer > 0)
            return;

        UpdateMove();
     }

    private void UpdateMove()
    {
        float moveDist = speed * Time.deltaTime;

        Vector3 delta = nextPos - transform.position;

        // if we reached or overshot, it's also time to change direction.
        if(moveDist >= delta.magnitude)
        {
            moveDist = delta.magnitude;
            NextMoveDirection();
            CalculateNextPosition();
        }

        Vector3 nextMove = delta.normalized * moveDist;

        transform.position += nextMove;
    }
}
