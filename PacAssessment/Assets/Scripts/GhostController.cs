using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public const float ScaredTimer = 10.0f;
    private const float recoverTimer = 3.0f;
    private const float deadTimer = 5.0f;
    private string[] ghostPositions = { "TopLeft_13_11", "TopRight_13_11", "BotLeft_13_11", "BotRight_13_11" };

    [SerializeField] private GameObject[] Ghosts = new GameObject[4];

    private Animator[] animatorController;
    private GameObject[] GhostSpriteGO;
    private SpriteRenderer[] spriteRenderer;
    private Map map;
    private Direction currentDirection;
    private Coroutine scaredCoroutine = null;
    private Coroutine[] deadCoroutine;
    private bool gameStart;

    public bool[] IsScared;
    public bool[] IsDead;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = true;
        IsScared = new bool[Ghosts.Length];
        IsDead = new bool[Ghosts.Length];
        deadCoroutine = new Coroutine[Ghosts.Length];
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (HUDAspect.IsStartTextActive)
            {
                foreach (Animator animator in animatorController)
                {
                    animator.speed = 0.0f;
                }
            }
            else
            {
                foreach (Animator animator in animatorController)
                {
                    animator.speed = 1.0f;
                }

                gameStart = false;
            }
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
        for (int i = 0; i < Ghosts.Length; i++)
        {
            resetGhost(ghostPositions[i], i);
        }
    }

    /// <summary>
    /// Set ghost at given position
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="i"></param>
    private void resetGhost(string pos, int i)
    {
        Vector3 startPos = map.GetPositionFromName(pos);
        Ghosts[i].transform.position = startPos;
        IsScared[i] = false;
        IsDead[i] = false;
    }

    /// <summary>
    /// Set instance of map
    /// </summary>
    public void SetMap(Map map)
    {
        this.map = map;
    }

    /// <summary>
    /// Start coroutine for scared mode for ghosts
    /// </summary>
    public void TriggerScaredMode()
    {
        if (scaredCoroutine != null)
        {
            StopCoroutine(scaredCoroutine);
            scaredCoroutine = null;
        }

        scaredCoroutine = StartCoroutine(scared());
    }

    private IEnumerator scared()
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            IsScared[i] = true;
        }

        foreach (Animator animator in animatorController)
        {
            animator.SetTrigger("ScaredTrigger");
        }

        yield return new WaitForSeconds(ScaredTimer - recoverTimer);

        foreach (Animator animator in animatorController)
        {
            animator.SetTrigger("TransitionToRecoverTrigger");
        }

        yield return new WaitForSeconds(recoverTimer);

        foreach (Animator animator in animatorController)
        {
            animator.SetTrigger("TransitionToNormalTrigger");
        }

        for (int i = 0; i < Ghosts.Length; i++)
        {
            IsScared[i] = false;
        }

        StopCoroutine(scaredCoroutine);
        scaredCoroutine = null;
    }

    /// <summary>
    /// Get index of ghost gameobject from array
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    public int GetIndexFromGameObject(GameObject go)
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            if (Ghosts[i] == go)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Transition ghost to dead state given index
    /// </summary>
    /// <param name="i"></param>
    public void KillGhost(int i)
    {
        if (deadCoroutine[i] == null)
        {
            deadCoroutine[i] = StartCoroutine(kill(i));
        }
    }

    /// <summary>
    /// IEnumerator to transition ghost to dead state
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private IEnumerator kill(int i)
    {
        animatorController[i].SetTrigger("DeadTrigger");

        yield return new WaitForSeconds(deadTimer);

        resetGhost(ghostPositions[i], i);

        animatorController[i].SetTrigger("TransitionToNormalTrigger");
    }
}