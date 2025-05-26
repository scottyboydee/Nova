using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string STRING_SCORE = "SCORE : ";
    private const string STRING_HISCORE = "HI-SCORE : ";

    [SerializeField]
    private TMP_Text textScore;

    [SerializeField]
    private TMP_Text textHiscore;

    private int score;
    private int hiscore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        setScore(0);
        setHiscore(0);
    }

    private void UpdateScoreGUI()
    {
        textScore.text = STRING_SCORE + score;
    }

    private void UpdateHiscoreGUI()
    {
        textHiscore.text = STRING_HISCORE + score;
    }

    private void setScore(int newScore)
    {
        score = newScore;
        UpdateScoreGUI();
    }

    private void setHiscore(int newHiscore)
    {
        hiscore = newHiscore;
        UpdateHiscoreGUI();
    }


    public int getPlayerScore()
    {
        return score;
    }
}
