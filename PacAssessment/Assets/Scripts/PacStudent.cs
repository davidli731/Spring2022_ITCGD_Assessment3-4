using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PacStudent : MonoBehaviour
{
    private Animator animatorController;
    private SpriteRenderer pacStudentSpriteRenderer;
    private Map map;
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

        Vector3 startingPos = map.GetPositionFromName("TopLeft_1_1");
        if (startingPos != Vector3.zero)
        {
            gameObject.transform.position = startingPos;
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
        PacStudentSpriteGO.transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
        pacStudentSpriteRenderer.flipX = false;
        pacStudentSpriteRenderer.flipY = false;
    }
}
