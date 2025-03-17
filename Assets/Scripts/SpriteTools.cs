using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeftRight
{
    Left,
    Right
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class SpriteTools : MonoBehaviour
{
    public static readonly Vector3[] DirectionToVector3 =
    {
        new Vector3( 0, 0, 0 ),
        new Vector3( 0, 1, 0 ),
        new Vector3( 0, -1, 0 ),
        new Vector3( -1, 0, 0 ),
        new Vector3( 1, 0, 0 ),
    };

    [SerializeField]
    private Canvas canvas;

    public Canvas Canvas { get { return canvas; } }

    public static SpriteTools Inst { get; private set; }

    public enum Enclose
    {
        Inside,
        Crossing,
        Outside
    }

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetMax( Direction direction, Rect rect)
    {
        float max = 0;

        switch( direction) 
        {
            case Direction.Left:
                break;
        }

        return max;
    }

    // TODO: cleanup this GPT filth
    public Enclose CheckEnclosure(RectTransform rect)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Get the corners of the RectTransform in world space
        Vector3[] rectCorners = new Vector3[4];
        rect.GetWorldCorners(rectCorners);

        // Get the corners of the Canvas in world space
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        // Create a bounding box for the canvas
        Bounds canvasBounds = new Bounds(canvasCorners[0], Vector3.zero);
        for (int i = 1; i < 4; i++)
        {
            canvasBounds.Encapsulate(canvasCorners[i]);
        }

        // Check if the rect is fully inside, partially inside, or outside the canvas bounds
        bool fullyInside = true;
        bool partiallyInside = false;

        foreach (Vector3 corner in rectCorners)
        {
            if (canvasBounds.Contains(corner))
            {
                partiallyInside = true;
            }
            else
            {
                fullyInside = false;
            }
        }

        if (fullyInside)
        {
            return Enclose.Inside;
        }
        else if (partiallyInside)
        {
            return Enclose.Crossing;
        }
        else
        {
            return Enclose.Outside;
        }
    }
}

public static class RectExtensions
{
    public static Rect PadRectBySpriteRect(this Rect bounds, Rect spriteRect)
    {
        return new Rect(bounds.xMin + spriteRect.width/2, bounds.yMin + spriteRect.height/2, bounds.width - spriteRect.width, bounds.height - spriteRect.height);
    }
}
