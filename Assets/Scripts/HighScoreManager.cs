using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    private Controls controls;

    [Header("UI")]
    public TextMeshProUGUI highScoreNames;
    public TextMeshProUGUI highScoreScores;
    public GameObject pressFireText;
    [SerializeField]
    private float BlinkSpeed = 0.3f;


    private int editingIndex = -1;
    private string editingName = "";
    private int cursorPosition = 0;
    private int charIndex = 0;
    private float blinkTimer = 0f;
    private bool showCursor = true;
    public static HighScoreManager Instance { get; private set; }

    private HighScoreData data;

    private static readonly char[] CHARSET =
    {
        ' ','A','B','C','D','E','F','G','H','I','J','K','L','M',
        'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
        '0','1','2','3','4','5','6','7','8','9',
        '-','!','?','.'
    };

    private void Start()
    {
        controls = new Controls();
        controls.Gameplay.Enable();

    }

    private void Awake()
    {
        Instance = this;

        data = HighScoreData.Instance;

    }

    private void OnEnable()
    {

        UpdateNamesDisplay();
        UpdateScoreDisplay();

        if (ScoreManager.RecentHighscore > data.GetLowestScore())
        {
            Debug.Log("New Highscore to submit! " + ScoreManager.RecentHighscore + " > " + data.GetLowestScore());
            pressFireText.SetActive(false);
            AddScore(ScoreManager.RecentHighscore);
        }
        else
        {
            Debug.Log("No new highscore to submit, recent was: " + ScoreManager.RecentHighscore);
        }

        ScoreManager.Instance?.ClearRecentHighscore();

    }

    public void AddScore(int score)
    {
        editingIndex = data.AddScore(score);
        editingName = "";
        cursorPosition = 0;
        charIndex = 0;
        blinkTimer = 0f;
        showCursor = true;

        UpdateNamesDisplay();
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        highScoreScores.text = "";
        for (int i = 0; i < data.Scores.Count; i++)
        {
            highScoreScores.text += $"{data.Scores[i].score}\n";
        }
    }

    private void MoveToSplashScreen()
    {
        Debug.Log("MoveToSplashScreen");
        LifeCycleManager.Instance?.SetStateAfterFadeToBlack(LifeCycleManager.State.Splash);
    }

    void Update()
    {
        if (editingIndex < 0)
        {
            if (controls.Gameplay.Fire.WasPressedThisFrame())
                MoveToSplashScreen();

            return;
        }

        blinkTimer += Time.deltaTime;
        if (blinkTimer >= BlinkSpeed)
        {
            blinkTimer = 0f;
            showCursor = !showCursor;
        }

        // Keyboard input
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // Backspace
            {
                if (editingName.Length > 0)
                {
                    editingName = editingName[..^1];
                    cursorPosition = Mathf.Max(0, cursorPosition - 1);
                }
            }
            else if (c == '\n' || c == '\r') // Enter
            {
                SubmitName();
                return;
            }
            else if (CHARSET.Contains(char.ToUpper(c)) && editingName.Length < data.MaxNameLength)
            {
                editingName += char.ToUpper(c);
                cursorPosition++;
                if (cursorPosition >= data.MaxNameLength)
                {
                    SubmitName();
                    return;
                }
            }
        }

        // Joystick input
        if (controls.Gameplay.Backspace.WasPressedThisFrame())
        {
            if (editingName.Length > 0)
            {
                editingName = editingName[..^1];
                cursorPosition = Mathf.Max(0, cursorPosition - 1);
            }
            ResetBlink();
        }
        else if (controls.Gameplay.NextChar.WasPressedThisFrame())
        {
            if (editingName.Length < data.MaxNameLength)
            {
                editingName += CHARSET[charIndex];
                cursorPosition++;
                charIndex = 0;
            }

            if (editingName.Length >= data.MaxNameLength)
            {
                SubmitName();
                return;
            }
        }
        else if (controls.Gameplay.LetterNext.WasPressedThisFrame())
        {
            //Debug.Log("LetterNext");
            charIndex = (charIndex + 1) % CHARSET.Length;
            ResetBlink();
        }
        else if (controls.Gameplay.LetterPrevious.WasPressedThisFrame())
        {
            //Debug.Log("LetterPrevious");
            charIndex = (charIndex - 1 + CHARSET.Length) % CHARSET.Length;
            ResetBlink();
        }

        UpdateNamesDisplay();
    }

    private void ResetBlink()
    {
        showCursor = true;
        blinkTimer = 0;
    }

    void SubmitName()
    {
        data.SubmitName(editingIndex, editingName);
        editingIndex = -1;
        data.SaveHighscores();
        UpdateNamesDisplay();
        pressFireText.SetActive(true);
    }

    void UpdateNamesDisplay()
    {
        highScoreNames.text = "";
        for (int i = 0; i < data.Scores.Count; i++)
        {
            string name = data.Scores[i].name;
            if (i == editingIndex)
            {
                string visible = editingName;
                if (visible.Length < data.MaxNameLength)
                {
                    char preview = showCursor ? CHARSET[charIndex] : '_';
                    visible += preview;
                }
                name = visible.PadRight(data.MaxNameLength);
            }
            else
            {
                name = name.PadRight(data.MaxNameLength);
            }

//            highscoreText.text += $"{i + 1}. {name} {scores[i].score}\n";
            highScoreNames.text += $"{name}\n";
        }
    }
}
