using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PacStudent : MonoBehaviour
{
    private static float duration = 1.0f;
    private static float marginalDistance = 0.05f;

    private Animator animatorController;
    private SpriteRenderer pacStudentSpriteRenderer;
    private Map map;
    private PacStudentTween tween;
    private string[] directions = { "Left", "Right", "Up", "Down" };
    private string currentDirection;

    [SerializeField] private GameObject PacStudentSpriteGO;
    [SerializeField] private Sprite idleSprite;

    [SerializeField] private LevelGenerator levelGenerator;

    public bool isWalking = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animatorController = PacStudentSpriteGO.GetComponent<Animator>();
        pacStudentSpriteRenderer = PacStudentSpriteGO.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            animatorController.speed = 1;

            handleMovement();
        } else
        {
            if (pacStudentSpriteRenderer.sprite == idleSprite)
                animatorController.speed = 0;
        }

        if (isDead)
        {
            animatorController.SetTrigger("DeadTrigger");
        }
    }

    /// <summary>
    /// Set sprite initial scale depending on map size
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetInitialScale(int x, int y)
    {
        float scaleX = PacStudentSpriteGO.transform.localScale.x;
        float scaleY = PacStudentSpriteGO.transform.localScale.y;
        float scaleZ = PacStudentSpriteGO.transform.localScale.z;

        scaleX = scaleX * (15.0f / y);
        scaleY = scaleY * (14.0f / x);

        PacStudentSpriteGO.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    /// <summary>
    /// Set initial position and direction
    /// </summary>
    public void Setup()
    {
        resetPacStudent();
        currentDirection = directions[1];
        isWalking = true;

        string startPosName = "TopLeft_1_1";
        Vector3 startPos = map.GetPositionFromName(startPosName);
        Vector3 destPos = map.GetNeighbourPosition(startPosName, currentDirection);

        if (startPos != Vector3.zero)
        {
            gameObject.transform.position = startPos;
        }

        tween = new PacStudentTween(startPos, destPos, Time.time);
    }

    /// <summary>
    /// Handle the player movement
    /// </summary>
    private void handleMovement()
    {
        if (tween != null)
        {
            string startPosName;
            Vector3 startPos;
            Vector3 destPos;

            float distance = Vector3.Distance(gameObject.transform.position, tween.DestPos);

            if (distance > marginalDistance)
            {
                gameObject.transform.position = Vector3.Lerp(tween.StartPos, tween.DestPos, (Time.time - tween.StartTime) / duration);
            } else
            {
                gameObject.transform.position = tween.DestPos;

                startPosName = map.GetNameFromPosition(gameObject.transform.position);
                startPos = map.GetPositionFromName(startPosName);

                if (currentDirection == directions[1] && startPosName == "TopLeft_1_12")
                {
                    resetPacStudent();
                    currentDirection = directions[3];
                    gameObject.transform.Rotate(0.0f, 0.0f, -90.0f);
                }
                else if (currentDirection == directions[3] && startPosName == "TopLeft_14_12")
                {
                    resetPacStudent();
                    currentDirection = directions[0];
                    gameObject.transform.rotation = Quaternion.identity;
                    pacStudentSpriteRenderer.flipX = true;
                    
                }
                else if (currentDirection == directions[0] && startPosName == "TopLeft_14_1")
                {
                    resetPacStudent();
                    currentDirection = directions[2];
                    gameObject.transform.Rotate(0.0f, 0.0f, 90.0f);
                }
                else if (currentDirection == directions[2] && startPosName == "TopLeft_1_1")
                {
                    resetPacStudent();
                    currentDirection = directions[1];
                }

                    destPos = map.GetNeighbourPosition(startPosName, currentDirection);

                tween = new PacStudentTween(startPos, destPos, Time.time);
            }
        }
    }

    /// <summary>
    /// Set instance of map
    /// </summary>
    public void SetMap(Map map)
    {
        this.map = map;
    }

    /// <summary>
    /// Reset to default rotation and facing right
    /// </summary>
    private void resetPacStudent()
    {
        gameObject.transform.rotation = Quaternion.identity;
        pacStudentSpriteRenderer.flipX = false;
        pacStudentSpriteRenderer.flipY = false;
    }
}
