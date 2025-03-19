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

        switch (direction)
        {
            case Direction.Up:
                transform.position += new Vector3(0, useSpeed, 0);
                if (transform.position.y > bounds.yMax)
                {
                    transform.position = new Vector3(transform.position.x, bounds.yMax, 0);
                    direction = Direction.Down;
                }
                break;
            case Direction.Down:
                transform.position -= new Vector3(0, useSpeed, 0);
                if (transform.position.y < bounds.yMin)
                {
                    transform.position = new Vector3(transform.position.x, bounds.yMin, 0);
                    direction = Direction.Up;
                }
                break;
            case Direction.Left:
                transform.position -= new Vector3(useSpeed, 0, 0);
                if (transform.position.x < bounds.xMin)
                {
                    transform.position = new Vector3(bounds.xMin, transform.position.y, 0);
                    direction = Direction.Right;
                }
                break;
            case Direction.Right:
                transform.position += new Vector3(useSpeed, 0, 0);
                if (transform.position.x > bounds.xMax)
                {
                    transform.position = new Vector3(bounds.xMax, transform.position.y, 0);
                    direction = Direction.Left;
                }
                break;
            default:
                Debug.LogError("Bouncing sprite behaviour has invalid direction.");
                break;
        }
    }
}
