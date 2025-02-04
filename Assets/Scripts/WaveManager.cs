using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] 
    private SO_WaveSet waveSet;

    void Start()
    {
        CreateAllWaves();
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
            Debug.Log("Creating Wave: " + wave.name);

            if (wave.wavePrefab == null)
            {
                Debug.LogError("WavePrefab is NULL");
                continue;
            }

            Debug.Log("Spawning Wave: " + wave.wavePrefab.name);
            Instantiate(wave.wavePrefab, Vector3.zero, Quaternion.identity);
        }

    }
}
