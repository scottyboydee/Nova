using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieBounce : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float yAccel;

    private RectTransform myTransform;

    [SerializeField]
    private LeftRight direction;

    [SerializeField]
    private AnimSprite animSprite;

    private Rect bounds;

    private float yVel = 0;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<RectTransform>();

        SetupBounds();

        SetAnimDirection();
    }

    private void SetAnimDirection()
    {
        if( direction == LeftRight.Left )
        {
            animSprite.SetReverse( true );
        }
        else
        {
            animSprite.SetReverse(false);
        }
    }

    private void SetupBounds()
    {
        bounds = SpriteTools.Inst.Canvas.pixelRect;

        bounds = bounds.PadRectBySpriteRect(myTransform.rect);
    }

    // Update is called once per frame
    void Update()
    {
        MoveLaterally();

        MoveVertically();

        //        Debug.Log("Baddie pos: " + myTransform.localPosition);
    }

    private void MoveVertically()
    {
        //        float useAccel = yAccel * Time.deltaTime;
        float useAccel = yAccel * 0.01f;

        yVel -= yAccel;

//        float useSpeed = yVel * Time.deltaTime;
        float useSpeed = yVel * 0.01f;

        myTransform.position += new Vector3(0, useSpeed, 0);

        if (myTransform.position.y <= bounds.yMin)
        {
            Debug.Log("Hit floor");
            yVel = -yVel;

            myTransform.position = new Vector3(myTransform.position.x, bounds.yMin, myTransform.position.z);
        }

    }

    private void MoveLaterally()
    {
        float useSpeed = speed * Time.deltaTime;

        switch (direction)
        {
            case LeftRight.Left:
                myTransform.position -= new Vector3(useSpeed, 0, 0);

                if (myTransform.position.x <= bounds.xMin)
                {
//                    Debug.Log("Hit left side");
                    myTransform.position = new Vector3(bounds.xMin, myTransform.position.y, myTransform.position.z);

                    direction = LeftRight.Right;
                    SetAnimDirection();
                }

                break;
            case LeftRight.Right:
                myTransform.position += new Vector3(useSpeed, 0, 0);

                if (myTransform.position.x >= bounds.xMax)
                {
//                    Debug.Log("Hit right side");
                    myTransform.position = new Vector3(bounds.xMax, myTransform.position.y, myTransform.position.z);

                    direction = LeftRight.Left;
                    SetAnimDirection();
                }

                break;
        }
    }
}
