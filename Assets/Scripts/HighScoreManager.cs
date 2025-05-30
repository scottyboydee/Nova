using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    [System.Serializable]
    public struct Entry
    {
        public int score;
        public string name;
    }

    private Controls controls;

    [Header("UI")]
    public TextMeshProUGUI highScoreNames;
    public TextMeshProUGUI highScoreScores;
    public GameObject pressFireText;

    [Header("Settings")]
    [SerializeField]
    private int maxEntries = 10;
    [SerializeField]
    private int maxNameLength = 15;
    [SerializeField]
    private float BlinkSpeed = 0.3f;

    [Header("Persistence")]
    public string[] defaultNameTable;
    public string defaultNameFallback = "AAA"; // Used if no name is saved
    public int DefaultHighScore = 1000;
    public bool forceResetScores = false;

    private List<Entry> scores = new();

    private int editingIndex = -1;
    private string editingName = "";
    private int cursorPosition = 0;
    private int charIndex = 0;
    private float blinkTimer = 0f;
    private bool showCursor = true;
    public static HighScoreManager Instance { get; private set; }

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

    public int GetTopHighScore()
    {
        if( scores == null || scores.Count == 0 )
        {
            Debug.LogError("EEEK. Highscores not loaded, but GetTopHighScore called.");
            return 0;
        }

        return scores[0].score;
    }

    private void Awake()
    {
        Instance = this;

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

    private void OnEnable()
    {

        UpdateNamesDisplay();
        UpdateScoreDisplay();

        if (ScoreManager.RecentHighscore > scores[scores.Count-1].score)
        {
            Debug.Log("New Highscore to submit! " + ScoreManager.RecentHighscore + " > " + scores[0].score);
            pressFireText.SetActive(false);
            AddScore(ScoreManager.RecentHighscore);
        }
        else
        {
            Debug.Log("No new highscore to submit, recent was: " + ScoreManager.RecentHighscore);
        }

        ScoreManager.Instance?.ClearRecentHighscore();

    }

    void SaveHighscores()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"HS_{i}_Score", scores[i].score);
            PlayerPrefs.SetString($"HS_{i}_Name", scores[i].name);
        }
        PlayerPrefs.Save();
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



    public void AddScore(int score)
    {
        Entry newEntry = new Entry { score = score, name = "" };
        scores.Add(newEntry);
        scores = scores.OrderByDescending(e => e.score).Take(maxEntries).ToList();

        editingIndex = scores.IndexOf(newEntry);
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
        for (int i = 0; i < scores.Count; i++)
        {
            highScoreScores.text += $"{scores[i].score}\n";
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
            else if (CHARSET.Contains(char.ToUpper(c)) && editingName.Length < maxNameLength)
            {
                editingName += char.ToUpper(c);
                cursorPosition++;
                if (cursorPosition >= maxNameLength)
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
            if (editingName.Length < maxNameLength)
            {
                editingName += CHARSET[charIndex];
                cursorPosition++;
                charIndex = 0;
            }

            if (editingName.Length >= maxNameLength)
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
        scores[editingIndex] = new Entry { score = scores[editingIndex].score, name = editingName };
        editingIndex = -1;
        SaveHighscores();
        UpdateNamesDisplay();
        pressFireText.SetActive(true);
    }

    void UpdateNamesDisplay()
    {
        highScoreNames.text = "";
        for (int i = 0; i < scores.Count; i++)
        {
            string name = scores[i].name;
            if (i == editingIndex)
            {
                string visible = editingName;
                if (visible.Length < maxNameLength)
                {
                    char preview = showCursor ? CHARSET[charIndex] : '_';
                    visible += preview;
                }
                name = visible.PadRight(maxNameLength);
            }
            else
            {
                name = name.PadRight(maxNameLength);
            }

//            highscoreText.text += $"{i + 1}. {name} {scores[i].score}\n";
            highScoreNames.text += $"{name}\n";
        }
    }
}
