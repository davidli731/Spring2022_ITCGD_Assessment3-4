using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDAspect : MonoBehaviour
{
    private const string hudTag = "HUD";
    private const string scoreTag = "HUDScore";
    private const string scoreText = "Score: ";
    private const string ghostTimerTag = "HUDScaredTimer";
    private const string startTextTag = "HUDStartText";
    private const string subTextTag = "HUDSubText";
    private const string endTextTag = "HUDEndText";
    private const string timerTag = "HUDTimer";
    private const string ghostTimerText = "Scared Timer: ";
    private const string timerText = "Timer: ";
    private const string saveScoreKey = "SaveScoreKey";
    private const string saveTotalTimeKey = "SaveTotalTimeKey";
    private const float gameOverDelay = 3.0f;
    private const float flickerTimer = 1.0f;

    public const float startTimerCountdownDelay = 1.0f;

    // set this to 4_3 or 16_9 to change aspect ratio
    private string defaultAspectRatio = "16_9";

    private GameObject hudGO;
    private GameObject[] LivesGO;
    private TextMeshProUGUI scoreTMP;
    private TextMeshProUGUI ghostScaredTimerTMP;
    private TextMeshProUGUI startTextTMP;
    private TextMeshProUGUI subTextTMP;
    private TextMeshProUGUI endTextTMP;
    private TextMeshProUGUI timerTMP;
    private Coroutine startTextCoroutine;
    private Coroutine endTextCoroutine;
    private Coroutine timerCoroutine;
    private Coroutine subTextCoroutine;
    private string[] startText = { "GO!", "1", "2", "3" };
    private string hoursText, minutesText, secondsText;
    private int hours, minutes, seconds;

    private static int scoreValue;
    private static int totalTime;

    [SerializeField] private GameObject hud4_3;
    [SerializeField] private GameObject hud16_9;

    public static bool IsTimerActive;
    public static bool IsStartTextActive;
    public static bool IsEndGame;
    public static int GhostTimerValue;
    public static int LifeCount;

    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 0;
        totalTime = 0;
        GhostTimerValue = 0;
        LifeCount = 3;
        IsTimerActive = false;
        IsStartTextActive = false;
        IsEndGame = false;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(hudTag);

        // Remove existing canvas
        foreach(GameObject dupe in gameObjects)
        {
            Destroy(dupe);
        }

        // Add new canvas
        if (defaultAspectRatio == "4_3")
        {
            hudGO = Instantiate(hud4_3);
        }
        else if (defaultAspectRatio == "16_9")
        {
            hudGO = Instantiate(hud16_9);
        }

        gameObjects = GameObject.FindGameObjectsWithTag(scoreTag);
        foreach(GameObject go in gameObjects)
        {
            scoreTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(ghostTimerTag);
        foreach (GameObject go in gameObjects)
        {
            ghostScaredTimerTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        LivesGO = new GameObject[LifeCount];
        for (int i = 1; i <= 3; i++)
        {
            gameObjects = GameObject.FindGameObjectsWithTag("Live" + i);
            foreach(GameObject go in gameObjects)
            {
                LivesGO[i - 1] = go;
            }
        }

        gameObjects = GameObject.FindGameObjectsWithTag(startTextTag);
        foreach (GameObject go in gameObjects)
        {
            startTextTMP = go.GetComponent<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(endTextTag);
        foreach (GameObject go in gameObjects)
        {
            endTextTMP = go.GetComponent<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(timerTag);
        foreach (GameObject go in gameObjects)
        {
            timerTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(subTextTag);
        foreach (GameObject go in gameObjects)
        {
            subTextTMP = go.GetComponentInChildren<TextMeshProUGUI>();
        }

        subTextTMP.enabled = false;

        if (startTextCoroutine == null)
        {
            startTextCoroutine = StartCoroutine(startTextCountdown());
        }
    }

    private void Update()
    {
        scoreTMP.text = scoreText + scoreValue;

        seconds = totalTime % 60;
        secondsText = seconds.ToString();
        minutes = totalTime / 60;
        minutesText = minutes.ToString();
        hours = totalTime / 3600;
        hoursText = hours.ToString();

        if (seconds < 10)
        {
            secondsText = 0 + secondsText;
        }
        
        if (minutes < 10)
        {
            minutesText = 0 + minutesText;
        }
        
        if (hours < 10)
        {
            hoursText = 0 + hoursText;
        }

        timerTMP.text = timerText + hoursText + ":" + minutesText + ":" + secondsText;

        if (IsTimerActive)
        {
            ghostScaredTimerTMP.gameObject.SetActive(true);
            ghostScaredTimerTMP.text = ghostTimerText + GhostTimerValue;
        }
        else
        {
            ghostScaredTimerTMP.gameObject.SetActive(false);
        }

        if (LifeCount <= 2)
        {
            checkAndUpdateLife(2);

            if (LifeCount <= 1)
            {
                checkAndUpdateLife(1);

                if (LifeCount == 0)
                {
                    checkAndUpdateLife(0);
                }
            }
        }

        if (!IsStartTextActive && !IsEndGame)
        {
            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(startTimer());
            }
        }

        if (IsEndGame)
        {
            if (endTextCoroutine == null)
            {
                endTextCoroutine = StartCoroutine(showGameOver());
            }
        }

        if (DoubleSpeedController.HasCollectedDoubleSpeed && !DoubleSpeedController.HasUsedDoubleSpeed)
        {
            if (subTextCoroutine == null)
            {
                subTextCoroutine = StartCoroutine(subTextFlicker());
            }
        }
        else
        {
            if (subTextCoroutine != null)
            {
                StopCoroutine(subTextCoroutine);
                subTextCoroutine = null;
            }

            subTextTMP.enabled = false;
        }
    }

    /// <summary>
    /// Update lives
    /// </summary>
    /// <param name="i"></param>
    private void checkAndUpdateLife(int i)
    {
        if (LivesGO[i].activeSelf)
        {
            LivesGO[i].SetActive(false);
        }
    }

    /// <summary>
    /// Add points to score
    /// </summary>
    /// <param name="point"></param>
    public static void AddPoints(Points point)
    {
        scoreValue += (int)point;
    }

    /// <summary>
    /// Countdown of timer when game initially starts
    /// </summary>
    /// <returns></returns>
    private IEnumerator startTextCountdown()
    {
        IsStartTextActive = true;

        for (int i = startText.Length - 1; i >= 0; i--)
        {
            startTextTMP.text = startText[i];
            yield return new WaitForSeconds(startTimerCountdownDelay);
        }

        startTextTMP.gameObject.SetActive(false);
        IsStartTextActive = false;

        StopCoroutine(startTextCoroutine);
        startTextCoroutine = null;
    }

    /// <summary>
    /// Update game timer by 1 second
    /// </summary>
    /// <returns></returns>
    private IEnumerator startTimer()
    {
        totalTime++;
        yield return new WaitForSeconds(1.0f);

        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }

    /// <summary>
    /// show game over text
    /// </summary>
    /// <returns></returns>
    private IEnumerator showGameOver()
    {
        updateScoreAndTime();
        endTextTMP.enabled = true;

        yield return new WaitForSeconds(gameOverDelay);

        StopCoroutine(endTextCoroutine);
        endTextCoroutine = null;

        SceneManager.LoadScene((int)GameState.StartScene);
    }

    /// <summary>
    /// Coroutine to flicker subText
    /// </summary>
    /// <returns></returns>
    private IEnumerator subTextFlicker()
    {
        subTextTMP.enabled = true;
        yield return new WaitForSeconds(flickerTimer);
        subTextTMP.enabled = false;
        yield return new WaitForSeconds(flickerTimer);

        StopCoroutine(subTextCoroutine);
        subTextCoroutine = null;
    }

    /// <summary>
    /// Update score and time
    /// </summary>
    private void updateScoreAndTime()
    {
        int getPreviousHighScore = PlayerPrefs.GetInt(saveScoreKey);
        int getPreviousTime = PlayerPrefs.GetInt(saveTotalTimeKey);

        if (getPreviousTime == 0 ||
            scoreValue > getPreviousHighScore ||
            (scoreValue == getPreviousHighScore && totalTime < getPreviousTime))
        {
            PlayerPrefs.SetInt(saveScoreKey, scoreValue);
            PlayerPrefs.SetInt(saveTotalTimeKey, totalTime);
        }
    }
}
