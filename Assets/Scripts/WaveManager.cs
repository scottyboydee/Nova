using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private ExplosionManager explosionManager; // Set in Inspector
    public ExplosionManager ExplosionManager => explosionManager; // Read-only public access

    [SerializeField]
    GameObject waveParent;

    [SerializeField] 
    private SO_WaveSet waveSet;

    private List<Baddie> baddies;

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
    }

    void Start()
    {
        CreateAllWaves();

        baddies = new List<Baddie>();
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

        if (waveSet == null)
        {
            Debug.LogError("NO WAVESET!!");
            return;
        }

        foreach (Wave wave in waveSet.waves)
        {
            SpawnWave( wave );
        }

    }

    private void SpawnWave( Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);

        GameObject newWave = wave.gameObject.Clone(waveParent.transform, wave.name, Vector3.zero);
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

//        Debug.Log("RemoveBaddieFromList: number of baddies now: " + baddies.Count);
    }
}
