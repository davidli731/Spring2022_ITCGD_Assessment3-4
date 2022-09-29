using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    private const float marginalDistance = 0.05f;

    private float duration;
    private float pacStudentOffset;

    private IEnumerator swapSpriteCoroutine;
    private int currentSpriteValue;
    [SerializeField] private Image ghostImage;
    [SerializeField] private Sprite[] ghostSprites;

    [SerializeField] private GameObject pacStudentGO;
    [SerializeField] private GameObject[] ghostGO;
    private PacStudentTween pacStudentTween;
    private GhostTween[] ghostTween = new GhostTween[4];
    private string[] directions = { "Left", "Right", "Up", "Down" };
    private string pacStudentCurrentDirection;
    private string[] ghostCurrentDirection = new string[4];

    private Vector3[] corners =
    {
        new Vector3(150.0f, -74.0f, 0.0f),
        new Vector3(150.0f, 20.0f, 0.0f),
        new Vector3(-150.0f, 20.0f, 0.0f),
        new Vector3(-150.0f, -74.0f, 0.0f),
    };

    // Start is called before the first frame update
    void Start()
    {
        currentSpriteValue = 0;

        swapSpriteCoroutine = swapSprite();
        StartCoroutine(swapSpriteCoroutine);

        testInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (swapSpriteCoroutine == null)
        {
            swapSpriteCoroutine = swapSprite();
            StartCoroutine(swapSpriteCoroutine);
        }

        handleMovement();
    }

    /// <summary>
    /// Coroutine to swap sprites
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator swapSprite()
    {
        yield return new WaitForSeconds(1.5f);

        currentSpriteValue++;

        if (currentSpriteValue > 2)
        {
            currentSpriteValue = 0;
        }

        ghostImage.sprite = ghostSprites[currentSpriteValue];

        StopCoroutine(swapSpriteCoroutine);
        swapSpriteCoroutine = null;
    }

    /// <summary>
    /// Load scene
    /// </summary>
    /// <param name="value"></param>
    public void LoadLevel(int value)
    {
        SceneManager.LoadScene(value);
    }

    /// <summary>
    /// Handle the player movement
    /// </summary>
    private void handleMovement()
    {
        if (pacStudentTween != null)
        {
            float distance = Vector3.Distance(pacStudentGO.transform.localPosition, pacStudentTween.DestPos);

            if (distance > marginalDistance)
            {
                pacStudentGO.transform.localPosition = Vector3.Lerp(pacStudentTween.StartPos, pacStudentTween.DestPos, (Time.time - pacStudentTween.StartTime) / duration);
            }
            else
            {
                pacStudentGO.transform.localPosition = pacStudentTween.DestPos;

                if (pacStudentCurrentDirection == directions[1] && pacStudentGO.transform.localPosition == corners[0])
                {
                    pacStudentCurrentDirection = directions[2];
                    duration = 4.0f;
                    pacStudentTween = new PacStudentTween(pacStudentGO.transform.localPosition, corners[1], Time.time);
                }
                else if (pacStudentCurrentDirection == directions[2] && pacStudentGO.transform.localPosition == corners[1])
                {
                    pacStudentCurrentDirection = directions[0];
                    duration = 10.0f;
                    pacStudentTween = new PacStudentTween(pacStudentGO.transform.localPosition, corners[2], Time.time);
                }
                else if (pacStudentCurrentDirection == directions[0] && pacStudentGO.transform.localPosition == corners[2])
                {
                    pacStudentCurrentDirection = directions[3];
                    duration = 4.0f;
                    pacStudentTween = new PacStudentTween(pacStudentGO.transform.localPosition, corners[3], Time.time);
                }
                else if (pacStudentCurrentDirection == directions[3] && pacStudentGO.transform.localPosition == corners[3])
                {
                    pacStudentCurrentDirection = directions[1];
                    duration = 10.0f;
                    pacStudentTween = new PacStudentTween(pacStudentGO.transform.localPosition, corners[0], Time.time);
                }
            }
        }
    }

    /// <summary>
    /// Set pacStudent and ghosts to initial position and initialise tween
    /// </summary>
    private void testInit()
    {
        pacStudentCurrentDirection = directions[1];
        pacStudentOffset = (Vector3.Distance(pacStudentGO.transform.localPosition, corners[0]) + 150.0f) / 300.0f;
        duration = 5.0f / pacStudentOffset;
        pacStudentTween = new PacStudentTween(pacStudentGO.transform.localPosition, corners[0], Time.time);

        for (int i = 0; i < 4; i++)
        {
            ghostCurrentDirection[i] = directions[1];
            ghostTween[i] = new GhostTween(corners[3], corners[0], Time.time);
        }
    }
}
