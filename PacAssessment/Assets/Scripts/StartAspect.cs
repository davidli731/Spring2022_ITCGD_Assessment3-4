using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartAspect : MonoBehaviour
{
    private const string startCanvasTag = "StartCanvas";
    private const string highScoreTag = "StartSceneHighScore";
    private const string timeTag = "StartSceneTime";
    private const string saveScoreKey = "SaveScoreKey";
    private const string saveTotalTimeKey = "SaveTotalTimeKey";
    private const string scoreText = "High Score: ";
    private const string timeText = "Time: ";

    private TextMeshProUGUI highScoreTMP;
    private TextMeshProUGUI timeTMP;
    private Button fourThreeButton;
    private Button sixteenNineButton;

    private string[] aspectRatioTag = { "4:3Tag", "16:9Tag" };

    // set this to 4_3 or 16_9 to change aspect ratio
    private static string defaultAspectRatio = "16_9";

    [SerializeField] private GameObject startCanvas4_3;
    [SerializeField] private GameObject startCanvas16_9;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    /// <summary>
    /// Initialise
    /// </summary>
    private void init()
    {
        GameObject[] dupes = GameObject.FindGameObjectsWithTag(startCanvasTag);

        // Remove existing canvas
        foreach (GameObject dupe in dupes)
        {
            Destroy(dupe);
        }

        // Add new canvas
        if (defaultAspectRatio == "4_3")
        {
            Instantiate(startCanvas4_3);
        }
        else if (defaultAspectRatio == "16_9")
        {
            Instantiate(startCanvas16_9);
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(highScoreTag);
        foreach (GameObject go in gameObjects)
        {
            highScoreTMP = go.GetComponent<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(timeTag);
        foreach (GameObject go in gameObjects)
        {
            timeTMP = go.GetComponent<TextMeshProUGUI>();
        }

        gameObjects = GameObject.FindGameObjectsWithTag(aspectRatioTag[(int)AspectRatio.FourThree]);
        foreach (GameObject go in gameObjects)
        {
            fourThreeButton = go.GetComponent<Button>();
        }
        fourThreeButton.onClick.AddListener(() => SetAspectRatio("4_3"));

        gameObjects = GameObject.FindGameObjectsWithTag(aspectRatioTag[(int)AspectRatio.SixteenNine]);
        foreach (GameObject go in gameObjects)
        {
            sixteenNineButton = go.GetComponent<Button>();
        }
        sixteenNineButton.onClick.AddListener(() => SetAspectRatio("16_9"));

        getHighScore();
        getTime();
    }

    /// <summary>
    /// Get previous high score
    /// </summary>
    private void getHighScore()
    {
        int scoreValue = PlayerPrefs.GetInt(saveScoreKey);
        highScoreTMP.text = scoreText + scoreValue;
    }

    /// <summary>
    /// Get previous best time
    /// </summary>
    private void getTime()
    {
        int totalTime = PlayerPrefs.GetInt(saveTotalTimeKey);
        int seconds = totalTime % 60;
        string secondsText = seconds.ToString();
        int minutes = totalTime / 60;
        string minutesText = minutes.ToString();
        int hours = totalTime / 3600;
        string hoursText = hours.ToString();

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

        timeTMP.text = timeText + hoursText + ":" + minutesText + ":" + secondsText;
    }

    /// <summary>
    /// Set aspect ratio for menu and game HUD
    /// Input needs to be "16_9" or "4_3"
    /// </summary>
    private void SetAspectRatio(string ratio)
    {
        defaultAspectRatio = ratio;
        HUDAspect.DefaultAspectRatio = ratio;
        init();
    }
}