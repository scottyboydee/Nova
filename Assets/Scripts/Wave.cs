using UnityEngine;

[System.Serializable]
public class Wave : MonoBehaviour
{
//    public GameObject wavePrefab; // Reference to the prefab for this wave

    private WaveManager waveManager;
    public void SetManager( WaveManager manager )
    {
        waveManager = manager;
    }

    private void Start()
    {
        Debug.Log("Wave spawned: " + gameObject.name);

    }
}

