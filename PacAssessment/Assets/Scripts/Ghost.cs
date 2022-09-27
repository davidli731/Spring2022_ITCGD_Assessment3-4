using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    
    [SerializeField] private GameObject[] Ghosts;

    private Animator[] animatorController;
    private GameObject[] GhostSpriteGO;
    private SpriteRenderer[] spriteRenderer;
    private Map map;
    private string[] directions = { "Left", "Right", "Up", "Down" };
    private string currentDirection;

    public bool isScared = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isScared)
        {
            //animatorController.SetTrigger("ScaredTrigger");
        }

        if (isDead)
        {
            //animatorController.SetTrigger("DeadTrigger");
        }
    }

    private void init()
    {
        GhostSpriteGO = new GameObject[Ghosts.Length];
        animatorController = new Animator[Ghosts.Length];
        spriteRenderer = new SpriteRenderer[Ghosts.Length];

        for (int i = 0; i < Ghosts.Length; i++)
        {
            GhostSpriteGO[i] = Ghosts[i].transform.Find("Sprite").gameObject;
            animatorController[i] = GhostSpriteGO[i].GetComponent<Animator>();
            spriteRenderer[i] = GhostSpriteGO[i].GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Set sprite initial scale depending on map size
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetInitialScale(int x, int y)
    {
        float scaleX;
        float scaleY;
        float scaleZ;

        foreach (GameObject sprite in GhostSpriteGO)
        {
            scaleX = sprite.transform.localScale.x;
            scaleY = sprite.transform.localScale.y;
            scaleZ = sprite.transform.localScale.z;

            scaleX = scaleX * (15.0f / y);
            scaleY = scaleY * (14.0f / x);

            sprite.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
    }

    /// <summary>
    /// Set initial position and direction
    /// </summary>
    public void Setup()
    {
        allGhostsReset();
    }

    /// <summary>
    /// All ghosts start at the centre of map
    /// </summary>
    private void allGhostsReset()
    {
        string startPosName;
        Vector3 startPos;

        startPosName = "TopLeft_13_11";
        startPos = map.GetPositionFromName(startPosName);
        Ghosts[0].transform.position = startPos;

        startPosName = "TopRight_13_11";
        startPos = map.GetPositionFromName(startPosName);
        Ghosts[1].transform.position = startPos;

        startPosName = "BotLeft_13_11";
        startPos = map.GetPositionFromName(startPosName);
        Ghosts[2].transform.position = startPos;

        startPosName = "BotRight_13_11";
        startPos = map.GetPositionFromName(startPosName);
        Ghosts[3].transform.position = startPos;
    }

    /// <summary>
    /// Set instance of map
    /// </summary>
    public void SetMap(Map map)
    {
        this.map = map;
    }
}
