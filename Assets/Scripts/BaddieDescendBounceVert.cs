using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaddieDescendBounceVert : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private RectTransform myTransform;

    private Rect bounds;

    private Direction direction = Direction.Down;

    // Start is called before the first frame update
    void Start()
    {
        bounds = SpriteTools.Inst.Canvas.pixelRect;

        bounds = bounds.PadRectBySpriteRect(myTransform.rect);
    }

    // Update is called once per frame
    void Update()
    {
        MoveVertically();
    }

    private void MoveVertically()
    {
        float useSpeed = speed * Time.deltaTime;

        if( direction == Direction.Down )
        {
            transform.position -= new Vector3(0, useSpeed, 0);
            if (transform.position.y < bounds.yMin)
            {
                transform.position = new Vector3(transform.position.x, bounds.yMin, 0);
                direction = Direction.Up;
            }
        }
        else
        {
            transform.position += new Vector3(0, useSpeed, 0);
            if (transform.position.y > bounds.yMax)
            {
                transform.position = new Vector3(transform.position.x, bounds.yMax, 0);
                direction = Direction.Down;
            }
        }

    }
}
