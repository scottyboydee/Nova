using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighScoreData : MonoBehaviour
{
    [System.Serializable]
    public struct Entry
    {
        public int score;
        public string name;
    }

    private List<Entry> scores = new();
    public List<Entry> Scores => scores;

    [Header("Settings")]
    [SerializeField]
    private int maxEntries = 10;
    [SerializeField]
    private int maxNameLength = 15;
    public int MaxNameLength => maxNameLength;

    [Header("Persistence")]
    public string[] defaultNameTable;
    public string defaultNameFallback = "AAA"; // Used if no name is saved
    public int DefaultHighScore = 1000;
    public bool forceResetScores = false;

    private static HighScoreData instance;
    public static HighScoreData Instance => instance;

    public int GetTopHighScore()
    {
        if (scores == null || scores.Count == 0)
        {
            Debug.LogError("EEEK. Highscores not loaded, but GetTopHighScore called.");
            return 0;
        }

        return scores[0].score;
    }

    public int GetLowestScore()
    {
        return scores[scores.Count - 1].score;
    }

    void ResetHighscores()
    {
        scores.Clear();

        for (int i = 0; i < maxEntries; i++)
        {
            string name = (i < defaultNameTable.Length && defaultNameTable[i] != null)
                ? defaultNameTable[i].ToUpperInvariant()
                : defaultNameFallback;

            // let's assume the wise editors of the project will not mess up the init data for this table
            //name = name.Length > maxNameLength ? name.Substring(0, maxNameLength) : name;

            scores.Add(new Entry { score = DefaultHighScore, name = name });
        }

        SaveHighscores();
    }

    public void SubmitName(int index, string name)
    {
        scores[index] = new Entry { score = scores[index].score, name = name };
        SaveHighscores();
    }

    public void SaveHighscores()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"HS_{i}_Score", scores[i].score);
            PlayerPrefs.SetString($"HS_{i}_Name", scores[i].name);
        }
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (forceResetScores || !PlayerPrefs.HasKey("HS_0_Score"))
        {
            ResetHighscores();
        }
        else
        {
            LoadHighscores();
        }

        forceResetScores = false;

    }

    public int AddScore(int score )
    {
        Entry newEntry = new Entry { score = score, name = "" };
        scores.Add(newEntry);
        scores = scores.OrderByDescending(e => e.score).Take(maxEntries).ToList();

        int index = scores.IndexOf(newEntry);

        return index;
    }
    void LoadHighscores()
    {

        scores.Clear();
        for (int i = 0; i < maxEntries; i++)
        {
            int score = PlayerPrefs.GetInt($"HS_{i}_Score", DefaultHighScore);
            string name = PlayerPrefs.GetString($"HS_{i}_Name", defaultNameFallback);
            scores.Add(new Entry { score = score, name = name });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
