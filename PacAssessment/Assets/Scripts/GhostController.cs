using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private const float duration = 0.5f;
    private const float marginalDistance = 0.05f;
    public const float ScaredTimer = 10.0f;
    private const float recoverTimer = 3.0f;
    private const float deadTimer = 5.0f;
    private string[] ghostPositions = { "TopLeft_13_11", "TopRight_13_11", "BotLeft_13_11", "BotRight_13_11" };
    private string[] sectors = { "TopLeft", "TopRight", "BotLeft", "BotRight" };
    private string[] spawnGrids = { "14_13", "14_12", "14_11", "13_13", "13_12", "13_11" };
    private string[] spawnAreaPositions;

    [SerializeField] private GameObject[] Ghosts = new GameObject[4];
    [SerializeField] private PacStudentController pacStudentController;

    private Animator[] animatorController;
    private GameObject[] GhostSpriteGO;
    public SpriteRenderer[] spriteRenderer;
    private Map map;
    private Direction[] previousDirection;
    private Direction[] currentDirection;
    private Coroutine scaredCoroutine = null;
    private Coroutine[] deadCoroutine;
    private bool gameStart;
    private float ghost1InitialDistance;
    private float ghost2InitialDistance;
    private GhostTween[] tween;

    public bool[] IsScared;
    public bool[] IsDead;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (HUDAspect.IsStartTextActive)
            {
                stopGhostWalking();
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
        else if (HUDAspect.IsEndGame)
        {
            stopGhostWalking();
        }
        else
        {
            handleMovement();
        }
    }

    private void init()
    {
        gameStart = true;
        IsScared = new bool[Ghosts.Length];
        IsDead = new bool[Ghosts.Length];
        deadCoroutine = new Coroutine[Ghosts.Length];
        GhostSpriteGO = new GameObject[Ghosts.Length];
        animatorController = new Animator[Ghosts.Length];
        spriteRenderer = new SpriteRenderer[Ghosts.Length];
        tween = new GhostTween[Ghosts.Length];
        previousDirection = new Direction[Ghosts.Length];
        currentDirection = new Direction[Ghosts.Length];

        for (int i = 0; i < Ghosts.Length; i++)
        {
            currentDirection[i] = Direction.None;
            GhostSpriteGO[i] = Ghosts[i].transform.Find("Sprite").gameObject;
            animatorController[i] = GhostSpriteGO[i].GetComponent<Animator>();
            spriteRenderer[i] = GhostSpriteGO[i].GetComponent<SpriteRenderer>();
        }

        initSpawnPositions();
    }

    /// <summary>
    /// Set an array of spawn positions
    /// </summary>
    private void initSpawnPositions()
    {
        spawnAreaPositions = new string[24];

        for (int i = 0; i < spawnAreaPositions.Length; i++)
        {
            for (int j = 0; j < sectors.Length; j++)
            {
                for (int k = 0; k < spawnGrids.Length; k++)
                {
                    spawnAreaPositions[i] = sectors[j] + "_" + spawnGrids[k];
                }
            }
        }
    }

    /// <summary>
    /// Handle the ghost movement
    /// </summary>
    private void handleMovement()
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            if ((int)Ghost.THREE == i)
            {
                if (tween[i] == null)
                {
                    findDirectionForGhost(i);
                }
                else
                {
                    float distance = Vector3.Distance(Ghosts[i].transform.position, tween[i].DestPos);

                    if (distance > marginalDistance)
                    {
                        Ghosts[i].transform.position = Vector3.Lerp(tween[i].StartPos, tween[i].DestPos, (Time.time - tween[i].StartTime) / duration);
                    }
                    else
                    {
                        Ghosts[i].transform.position = tween[i].DestPos;
                        tween[i] = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Find a direction for ghost
    /// </summary>
    private void findDirectionForGhost(int i)
    {
        string currentPosName = map.GetNameFromPosition(Ghosts[i].transform.position);

        if (numOfValidDirections(currentPosName) > 1)
        {
            previousDirection[i] = currentDirection[i];

            if (map.GetNameFromPosition(Ghosts[i].transform.position) == "TopLeft_14_0")
            {
                currentDirection[i] = Direction.Right;
            }
            else if (map.GetNameFromPosition(Ghosts[i].transform.position) == "TopRight_14_0") {
                currentDirection[i] = Direction.Left;
            }
            else
            {
                currentDirection[i] = getRandomDirection();

                while (!isDirectionValid(currentPosName, currentDirection[i]) ||
                    (isDirectionValid(currentPosName, currentDirection[i]) && isBacktrack(currentDirection[i], previousDirection[i])))
                {
                    currentDirection[i] = getRandomDirection();
                }
            }
        }
        else
        {
            currentDirection[i] = backtrack(previousDirection[i]);
        }

        Vector3 destPos = map.GetNeighbourPosition(currentPosName, currentDirection[i]);
        tween[i] = new GhostTween(Ghosts[i].transform.position, destPos, Time.time);
        handleRotateAndFlip(i);
    }

    /// <summary>
    /// handle rotate and flipping of ghost to face the correct direction
    /// </summary>
    /// <param name="i"></param>
    private void handleRotateAndFlip(int i)
    {
        resetGhost(i);

        if (currentDirection[i] == Direction.Up)
        {
            Ghosts[i].transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if (currentDirection[i] == Direction.Down)
        {
            Ghosts[i].transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        }
        else if (currentDirection[i] == Direction.Left)
        {
            spriteRenderer[i].flipX = true;
        }
    }

    /// <summary>
    /// Reset rotation and flip
    /// </summary>
    /// <param name="i"></param>
    private void resetGhost(int i)
    {
        Ghosts[i].transform.rotation = Quaternion.identity;
        spriteRenderer[i].flipX = false;
        spriteRenderer[i].flipY = false;
    }

    /// <summary>
    /// Get random direction
    /// </summary>
    /// <returns></returns>
    private Direction getRandomDirection()
    {
        return (Direction)Random.Range(0, 4);
    }

    /// <summary>
    /// Number of valid directions for ghosts from current position
    /// </summary>
    /// <returns></returns>
    private int numOfValidDirections(string posName)
    {
        int counter = 0;
        if (isDirectionValid(posName, Direction.Left)) counter++;
        if (isDirectionValid(posName, Direction.Right)) counter++;
        if (isDirectionValid(posName, Direction.Up)) counter++;
        if (isDirectionValid(posName, Direction.Down)) counter++;
        return counter;
    }

    /// <summary>
    /// Return bool to determine whether direction is valid
    /// </summary>
    /// <param name="posName"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool isDirectionValid(string posName, Direction direction)
    {
        return map.GetNeighbourPosition(posName, direction) != Vector3.zero &&
            !map.IsWallTypeFromPosition(map.GetNeighbourPosition(posName, direction));
    }

    /// <summary>
    /// Return bool to determine if it's a backtrack
    /// </summary>
    /// <param name="currentDirection"></param>
    /// <returns></returns>
    private bool isBacktrack(Direction currentDirection, Direction previousDirection)
    {
        if (currentDirection == Direction.Left && previousDirection == Direction.Right) return true;
        if (currentDirection == Direction.Right && previousDirection == Direction.Left) return true;
        if (currentDirection == Direction.Up && previousDirection == Direction.Down) return true;
        if (currentDirection == Direction.Down && previousDirection == Direction.Up) return true;
        return false;
    }

    /// <summary>
    /// Backtracking given a previous direction
    /// </summary>
    /// <param name="previousDirection"></param>
    /// <returns></returns>
    private Direction backtrack(Direction previousDirection)
    {
        if (previousDirection == Direction.Left)
        {
            return Direction.Right;
        }
        else if (previousDirection == Direction.Right)
        {
            return Direction.Left;
        }
        else if (previousDirection == Direction.Up)
        {
            return Direction.Down;
        }
        return Direction.Up;
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

        if (i == (int)Ghost.ONE)
        {
            ghost1InitialDistance = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.gameObject.transform.position);
        }
        else if (i == (int)Ghost.TWO)
        {
            ghost2InitialDistance = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.gameObject.transform.position);
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

    /// <summary>
    /// Stop ghost animator speed
    /// </summary>
    private void stopGhostWalking()
    {
        foreach (Animator animator in animatorController)
        {
            animator.speed = 0.0f;
        }
    }
}