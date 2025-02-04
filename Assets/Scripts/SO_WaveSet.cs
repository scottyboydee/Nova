using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveSet", menuName = "ScriptableObjects/WaveSet")]
public class SO_WaveSet : ScriptableObject
{
    public Wave[] waves;
}
