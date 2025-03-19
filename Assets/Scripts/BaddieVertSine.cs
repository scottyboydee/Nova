using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaddieVertSine : MonoBehaviour
{
    // you know what, let's calculate this, because the editor keeps messing things up, weirdly.
//    [SerializeField]
    LeftRight Side;

    [SerializeField]
    private float speed = 100;

    [SerializeField]
    private float sineOffset = 0;

    [SerializeField]
    private float sineScale = 0.1f;

    [SerializeField]
    private float sineMagnitude = 100;

    private float xStartPos;

    private float spritePadSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        xStartPos = transform.position.x;

        if (xStartPos < SpriteTools.Inst.Canvas.pixelRect.width/2)
        {
            Side = LeftRight.Left;
        }
        else
        {
            Side = LeftRight.Right;
        }

        // TODO: lazy :P refactor
        Image image = GetComponent<Image>();
        if(image != null )
        {
            spritePadSize = image.rectTransform.rect.width / 2;
//            Debug.Log("Got spritePadSize: "+ spritePadSize);
        }
        
    }

    private void MoveVertically()
    {
        float useSpeed = speed * Time.deltaTime;

        transform.position += new Vector3(0, -useSpeed, 0);

    }

    private void MoveHorizontally()
    {
        float yMax = SpriteTools.Inst.Canvas.pixelRect.yMax;

        float yDelta = yMax - transform.position.y;

        yDelta += sineOffset;

        yDelta *= sineScale;
        //        Debug.Log("yDelta: " + yDelta + " xOffset: " + xOffset);

        float xOffset = Mathf.Sin(yDelta);

        // cause a unipolar behaviour, rather than oscillating around the centroid
        xOffset *= sineMagnitude/2;

        xOffset += sineMagnitude/2;

        if( Side == LeftRight.Left)
        {
            xOffset *= -1;
        }

        transform.position = new Vector3( xStartPos + xOffset, transform.position.y, 0);

        WrapAtBottomOfScreen();
    }

    private void WrapAtBottomOfScreen()
    {
        float yTest = transform.position.y + spritePadSize;

        if (yTest > 0)
            return;

        float yReappear = SpriteTools.Inst.Canvas.pixelRect.height + spritePadSize*2;

        // this is REALLY annoying, the sprite needs to fully disappear, then teleport to JUST offscreen
        transform.position += new Vector3(0, yReappear, 0);

    }

    // Update is called once per frame
    void Update()
    {
        MoveVertically();

        MoveHorizontally();
    }
}
