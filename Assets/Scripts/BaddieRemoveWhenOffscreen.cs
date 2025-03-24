using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieRemoveWhenOffscreen : MonoBehaviour
{
    [SerializeField]
    private Baddie myBaddie;

    [SerializeField]
    private RectTransform spriteRect;

    private bool hasBeenOnScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckOffscreen();
    }

    private void CheckOffscreen()
    {
        SpriteTools.Enclose enclosed = SpriteTools.Inst.CheckEnclosure(spriteRect);

        if (hasBeenOnScreen == false)
        {
            if (enclosed != SpriteTools.Enclose.Inside)
                return;

            hasBeenOnScreen = true;
            return;
        }

        if (enclosed != SpriteTools.Enclose.Outside)
            return;

        myBaddie.RemoveYourself();
    }
}
