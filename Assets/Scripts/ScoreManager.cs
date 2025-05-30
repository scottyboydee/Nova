using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string STRING_SCORE = "SCORE : ";
    private const string STRING_HISCORE = "HI-SCORE : ";

    [SerializeField]
    private int[] ScoreByValue;

    private int[] ScoreByValueDefaults =
    {
        1,
        10,
        100,
        1000
    };

    [SerializeField]
    private TMP_Text textScore;

    [SerializeField]
    private TMP_Text textHiscore;

    private int score;
    private int highScore;

    private static int recentHighscore;
    public static int RecentHighscore => recentHighscore;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (ScoreByValue == null || ScoreByValue.Length == 0)
        {
            Debug.LogError("No scores for values found - using defaults, but please fix");
            ScoreByValue = ScoreByValueDefaults;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        setScore(0);

        int highscore = HighScoreManager.Instance.GetTopHighScore();

        setHighscore(highscore);
    }

    private void UpdateScoreGUI()
    {
        textScore.text = STRING_SCORE + score;
    }

    private void UpdateHighScoreGUI()
    {
        textHiscore.text = STRING_HISCORE + highScore;
    }

    private void setScore(int newScore)
    {
        score = newScore;
        recentHighscore = score;

        UpdateScoreGUI();

        UpdateHighScore();
    }

    private void UpdateHighScore()
    {
        if (highScore > score)
            return;

        setHighscore(score);
    }

    private void setHighscore(int newHiscore)
    {
        highScore = newHiscore;
        UpdateHighScoreGUI();
    }

    public void AddScoreByValue(Baddie.ScoreValue scoreValue)
    {
        int addToScore = ScoreByValue[(int)scoreValue];
        setScore(score + addToScore);
    }

    public int getPlayerScore()
    {
        return score;
    }

    public void ClearRecentHighscore()
    {
        Debug.Log("Clearing recent highscore of: " + recentHighscore);
        recentHighscore = 0;
    }
}
