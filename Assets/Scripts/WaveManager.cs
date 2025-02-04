using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    GameObject waveParent;

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
            Debug.Log("Spawning Wave: " + wave.name);

            GameObject newWave = wave.gameObject.Clone(waveParent.transform, wave.name, Vector3.zero);
            RectTransform rectTransform = (RectTransform)newWave.transform;
            rectTransform.anchoredPosition = Vector2.zero;
        }

    }
}
