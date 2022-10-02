using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    private const float marginalDistance = 0.05f;
    private const int arraySize = 5;

    private IEnumerator swapSpriteCoroutine;
    private int currentSpriteValue;
    [SerializeField] private Image ghostImage;
    [SerializeField] private Sprite[] ghostSprites;
    [SerializeField] private GameObject[] tsObjectGO = new GameObject[arraySize];
    [SerializeField] private SpriteRenderer[] tsObjectRenderer = new SpriteRenderer[arraySize];

    private DemoTween[] tsObjectTween = new DemoTween[arraySize];

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
        yield return new WaitForSeconds(0.75f);

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
        for (int i = 0; i < arraySize; i++)
        {
            if (tsObjectTween[i] != null)
            {
                float distance = Vector3.Distance(tsObjectGO[i].transform.localPosition, tsObjectTween[i].DestPos);

                if (distance > marginalDistance)
                {
                    tsObjectGO[i].transform.localPosition = Vector3.Lerp(tsObjectTween[i].StartPos, tsObjectTween[i].DestPos, (Time.time - tsObjectTween[i].StartTime) / tsObjectTween[i].Duration);
                }
                else
                {
                    tsObjectGO[i].transform.localPosition = tsObjectTween[i].DestPos;

                    Direction direction;
                    float duration;

                    if (tsObjectTween[i].Direction == Direction.Right && tsObjectGO[i].transform.localPosition == corners[0])
                    {
                        // Bottom right corner
                        direction = Direction.Up;
                        duration = 4.0f;

                        reset(i);
                        tsObjectGO[i].transform.Rotate(0.0f, 0.0f, 90.0f);

                        tsObjectTween[i] = new DemoTween(tsObjectGO[i].transform.localPosition, corners[1], Time.time, duration, direction);
                    }
                    else if (tsObjectTween[i].Direction == Direction.Up && tsObjectGO[i].transform.localPosition == corners[1])
                    {
                        // Top right corner
                        direction = Direction.Left;
                        duration = 10.0f;

                        reset(i);
                        tsObjectRenderer[i].flipX = true;

                        tsObjectTween[i] = new DemoTween(tsObjectGO[i].transform.localPosition, corners[2], Time.time, duration, direction);
                    }
                    else if (tsObjectTween[i].Direction == Direction.Left && tsObjectGO[i].transform.localPosition == corners[2])
                    {
                        // Top left corner
                        direction = Direction.Down;
                        duration = 4.0f;

                        reset(i);
                        tsObjectGO[i].transform.Rotate(0.0f, 0.0f, -90.0f);

                        tsObjectTween[i] = new DemoTween(tsObjectGO[i].transform.localPosition, corners[3], Time.time, duration, direction);
                    }
                    else if (tsObjectTween[i].Direction == Direction.Down && tsObjectGO[i].transform.localPosition == corners[3])
                    {
                        // Bottom left corner
                        direction = Direction.Right;
                        duration = 10.0f;

                        reset(i);

                        tsObjectTween[i] = new DemoTween(tsObjectGO[i].transform.localPosition, corners[0], Time.time, duration, direction);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set pacStudent and ghosts to initial position and initialise tween
    /// </summary>
    private void testInit()
    {
        Direction direction;
        float duration;
        float offset;

        for (int i = 0; i < arraySize; i++)
        {
            tsObjectRenderer[i] = tsObjectGO[i].GetComponentInChildren<SpriteRenderer>();

            direction = Direction.Right;
            offset = (Vector3.Distance(tsObjectGO[i].transform.localPosition, corners[0]) + 150.0f) / 300.0f;
            duration = (5.0f / offset) + (i * 4);
            tsObjectTween[i] = new DemoTween(tsObjectGO[i].transform.localPosition, corners[0], Time.time, duration, direction);
        }
    }

    /// <summary>
    /// Reset rotation and flip
    /// </summary>
    private void reset(int i)
    {
        tsObjectGO[i].transform.rotation = Quaternion.identity;
        tsObjectRenderer[i].flipX = false;
        tsObjectRenderer[i].flipY = false;
    }
}