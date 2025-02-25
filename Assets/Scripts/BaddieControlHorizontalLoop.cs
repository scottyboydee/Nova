using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieControlHorizontalLoop : MonoBehaviour
{
    [SerializeField]
    private LeftRight startDirection = LeftRight.Right;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float yMove;

    private RectTransform myTransform;

    private Direction direction;

    private Rect bounds;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<RectTransform>();

        SetupBounds();

        if( startDirection == LeftRight.Left )
        {
            direction = Direction.Left;
        }
        else
        {
            direction = Direction.Right;
        }

    }

    private void SetupBounds()
    {
        bounds = SpriteTools.Inst.Canvas.pixelRect;

        bounds = bounds.PadRectBySpriteRect(myTransform.rect);

        bounds.yMax = myTransform.position.y;
        bounds.yMin = myTransform.position.y - yMove;
    }

    private void MoveUpDown()
    {
        if (myTransform.position.y == bounds.yMax)
        {
            direction = Direction.Down;
        }
        else
        {
            direction = Direction.Up;
        }
    }

    private void MoveLeftRight()
    {
        if (myTransform.position.x == bounds.xMin)
        {
            direction = Direction.Right;
        }
        else
        {
            direction = Direction.Left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float useSpeed = speed * Time.deltaTime;

        switch (  direction ) 
        {
            case Direction.Up:
                myTransform.position += new Vector3(0, useSpeed, 0);

                if (myTransform.position.y >= bounds.yMax)
                {
//                    Debug.Log("Hit upper side");
                    myTransform.position = new Vector3(myTransform.position.x, bounds.yMax, myTransform.position.z);
                    MoveLeftRight();
                }

                break;
            case Direction.Down:
                myTransform.position -= new Vector3(0, useSpeed, 0);

                if (myTransform.position.y <= bounds.yMin)
                {
//                    Debug.Log("Hit lower side");
                    myTransform.position = new Vector3(myTransform.position.x, bounds.yMin, myTransform.position.z);
                    MoveLeftRight();
                }

                break;
            case Direction.Left:
                myTransform.position -= new Vector3(useSpeed, 0, 0);

                if( myTransform.position.x <= bounds.xMin )
                {
//                    Debug.Log("Hit left side");
                    myTransform.position = new Vector3( bounds.xMin, myTransform.position.y, myTransform.position.z );

                    MoveUpDown();
                }

                break;
            case Direction.Right:
                myTransform.position += new Vector3(useSpeed, 0, 0);

                if (myTransform.position.x >= bounds.xMax)
                {
//                    Debug.Log("Hit right side");
                    myTransform.position = new Vector3(bounds.xMax, myTransform.position.y, myTransform.position.z);
                    MoveUpDown();
                }

                break;

        }

//        Debug.Log("Baddie pos: " + myTransform.localPosition);
    }
}
