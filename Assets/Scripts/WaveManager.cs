using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] 
    private ExplosionManager explosionManager; // Set in Inspector
    public ExplosionManager ExplosionManager => explosionManager; // Read-only public access

    [SerializeField] 
    private BulletManager bulletManager; // Set in Inspector
    public BulletManager BulletManager => bulletManager; // Read-only public access

    [SerializeField]
    private int StartWave = 0;

    [SerializeField]
    private GameObject waveParent;

    [SerializeField]
    private float pauseBetweenWaves;

    [SerializeField]
    private GameObject waveBuilderPrefab;

//    [SerializeField] 
//    private SO_WaveSet waveSet;

    [SerializeField]
    private Wave[] waveBuilder;

    private List<Baddie> baddies;
    public List<Baddie> Baddies => baddies;

    private int nextWaveNum = 0;

    private float nextWavePauseRemaining = 0;


    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("EEK! WaveManager Singleton already existed!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // QoL so I don't have to keep turning subhierarchies on and off all the time in editor
        waveBuilderPrefab?.SetActive(false);

        baddies = new List<Baddie>();
    }

    void Start()
    {
        nextWaveNum = StartWave;

        NextWave();

//        CreateAllWaves();
    }

    public void AddBaddieToList( Baddie baddie )
    {
        if ( baddies == null ) 
        {
            Debug.Log("EEK! AddBaddieToList: Baddies list is null!");
            return;
        }

        if( baddie == null )
        {
            Debug.Log("EEK! AddBaddieToList: baddie was null!");
            return;
        }

        baddies.Add( baddie );

//        Debug.Log("AddBaddieToList: number of baddies now: " + baddies.Count );
    }

    private void CreateAllWaves()
    {
        Debug.Log("Creating ALL waves for testing...");

/*
        if (waveSet == null)
        {
            Debug.LogError("NO WAVESET!!");
            return;
        }
*/

        foreach (Wave wave in waveBuilder)
        {
            SpawnWave( wave );
        }

    }

    private void SpawnWave( Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);

        GameObject newWave = wave.gameObject.Clone(waveParent.transform, wave.name, Vector3.zero);
        newWave.SetActive(true);
        RectTransform rectTransform = (RectTransform)newWave.transform;
        rectTransform.anchoredPosition = Vector2.zero;
    }

    internal void RemoveBaddieFromList(Baddie baddie)
    {
        if (baddies == null)
        {
            Debug.Log("EEK! RemoveBaddieFromList: Baddies list is null!");
            return;
        }

        if (baddie == null)
        {
            Debug.Log("EEK! RemoveBaddieFromList: baddie was null!");
            return;
        }

        baddies.Remove(baddie);

//        if (baddies.Count > 0)
//            return;

//        Debug.Log("WaveManager: Wave complete, moving to next.");
//        StartPauseBeforeNextWave();


//        Debug.Log("RemoveBaddieFromList: number of baddies now: " + baddies.Count);
    }

    private void StartPauseBeforeNextWave()
    {
//        Debug.Log("StartPauseBeforeNextWave: setting to: " + pauseBetweenWaves);
        nextWavePauseRemaining = pauseBetweenWaves;
    }

    private void NextWave()
    {
        Debug.Log("NextWave: " + nextWaveNum);

        if( nextWaveNum >= waveBuilder.Length )
        {
            GameManager.Instance.GameComplete();
            Debug.Log("ALL WAVES COMPLETED! GAME OVER! BUT IN THE GOOD WAY!");
            return;
        }

        Debug.Log("Spawning Wave num: " + nextWaveNum);
        SpawnWave(waveBuilder[nextWaveNum]);

        nextWaveNum++;
    }

    private void Update()
    {
        if (GameManager.Instance.PlayerRespawning)
        {
//            Debug.Log("Player died so waiting...");
            return;
        }

        if( nextWavePauseRemaining > 0 )
        {
//            Debug.Log("Next Wave timer remain: " + nextWavePauseRemaining);
            nextWavePauseRemaining -= Time.deltaTime;

            if( nextWavePauseRemaining < 0 )
            {
//                Debug.Log("Next Wave timer depleted! Spawning!");
                nextWavePauseRemaining = 0;
                NextWave();
            }

            return;
        }

        if (baddies.Count > 0)
            return;

        Debug.Log("WaveManager: Wave complete, moving to next.");
        StartPauseBeforeNextWave();
    }

    public void RestartCurrentWave()
    {
        Debug.Log("RestartCurrentWave: ");

        nextWaveNum--;

        CleanUpAllWaves();

        NextWave();
    }

    private void CleanUpAllWaves()
    {
        foreach (Transform child in waveParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        baddies.Clear();
    }


}
