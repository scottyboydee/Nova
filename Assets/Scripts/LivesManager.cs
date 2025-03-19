using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lifePrefab;

    [SerializeField]
    private int MaxLives;

    [SerializeField]
    private List<GameObject>lifeIcons = new List<GameObject>();

    private int lives;

    private void Reset()
    {
//        Debug.Log("LivesManager: Reset");

        foreach (var item in lifeIcons) 
        {
            item.gameObject.SetActive(true);
        }

        lives = MaxLives;
    }

    private void CreateIcons()
    {
        while(lifeIcons.Count < (MaxLives-1)) 
        {
            GameObject newIcon = lifePrefab.Clone(transform);
            newIcon.SetActive(true);
            lifeIcons.Add(newIcon);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateIcons();

        Reset();
    }

    public bool RemoveLife()
    {
        Debug.Log("RemoveLife: previously had lives: " + lives);

        lives--;

        for(int i = 0; i < lifeIcons.Count; i++ )
        {
            if(lifeIcons[i].activeSelf)
            {
                lifeIcons[i].SetActive(false);
                break;
            }
        }

        if( lives <= 0 )
        {
            lives = 0;
            return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
