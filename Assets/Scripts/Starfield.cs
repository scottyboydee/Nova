using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Starfield : MonoBehaviour
{
    [SerializeField]
    private Image starPixelPrefab;

    [SerializeField]
    private RectTransform starfieldRect;

    [SerializeField]
    private int numStars = 50;

    [SerializeField]
    private float SpeedMin = 10f, SpeedMax = 100f;
    private float SpeedRange;

    [SerializeField]
    private float BrightMin = 0.2f, BrightMax = 0.7f;
    private float BrightRange;

    private Image[] starPixels;
    private float[] starSpeed;

    float xMax;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        xMax = starfieldRect.rect.width;
        yMax = starfieldRect.rect.height;

        starPixels = new Image[numStars];
        starSpeed = new float[numStars];

        BrightRange = BrightMax - BrightMin;
        SpeedRange = SpeedMax - SpeedMin;

        Debug.Log("Starfield: Start");
        Debug.Log("BrightMax: " + BrightMax + " BrightMin: " + BrightMin + " BrightRange: " + BrightRange);
        Debug.Log("SpeedMax: " + SpeedMax + " SpeedMin: " + SpeedMin + " SpeedRange: " + SpeedRange);

        CreateStars();
    }

    private void CreateStars()
    {
        Debug.Log("CreateStars: " + xMax + ":" + yMax);

        for ( int i = 0; i < numStars; i++ ) 
        {
            GameObject newStar = starPixelPrefab.gameObject.Clone(transform);
            newStar.SetActive(true);
            Image newStarImage = newStar.GetComponent<Image>();

            newStarImage.transform.position = new Vector3(Random.value*xMax, Random.value*yMax, 0);

            starPixels[i] = newStarImage;

            RandomiseStar(i);
        }

    }

    private void RandomiseStar( int i )
    {
        float randomUnit = Random.value;

        float randomSpeed = SpeedRange * randomUnit;
        randomSpeed += SpeedMin;

        starSpeed[i] = randomSpeed;

        float randomBright = BrightRange * randomUnit;
        randomBright += BrightMin;

        starPixels[i].color = new Color(randomBright, randomBright, randomBright);

//        Debug.Log("Randomised Star: " + i + " randomUnit: " + randomUnit + " randomBright: " + randomBright + " randomSpeed: " + randomSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllStars();        
    }

    private void UpdateAllStars()
    {
        float useSpeed;

        for( int i = 0; i < numStars; i++ )
        {
            useSpeed = Time.deltaTime * starSpeed[i];

            starPixels[i].transform.position += new Vector3(0, -useSpeed, 0);

            if(starPixels[i].transform.position.y < 0 )
            {
                starPixels[i].transform.position = new Vector3(Random.value * xMax, yMax, 0);
            }
        }
    }
}
