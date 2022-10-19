using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private const float duration = 0.5f;
    private const float marginalDistance = 0.05f;
    public const float ScaredTimer = 10.0f;
    private const float recoverTimer = 3.0f;
    private string[] ghostPositions = { "TopLeft_13_11", "TopRight_13_11", "BotLeft_13_11", "BotRight_13_11" };

    [SerializeField] private GameObject[] Ghosts = new GameObject[4];
    [SerializeField] private PacStudentController pacStudentController;
    [SerializeField] private SpawnAreaPositions spawnAreaPositions;

    private float[] deadTimer;
    private Animator[] animatorController;
    private GameObject[] GhostSpriteGO;
    public SpriteRenderer[] spriteRenderer;
    private Map map;
    private Direction[] previousDirection;
    private Direction[] currentDirection;
    private Coroutine scaredCoroutine = null;
    private Coroutine[] deadCoroutine;
    private bool gameStart;
    private float[] ghostInitialDistance;
    private GhostTween[] tween;
    private GhostOneInstructions ghostOneInstructions;
    private GhostTwoInstructions ghostTwoInstructions;
    private GhostThreeInstructions ghostThreeInstructions;
    private GhostFourInstructions ghostFourInstructions;

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

    /// <summary>
    /// Initialise variables
    /// </summary>
    private void init()
    {
        gameStart = true;
        deadTimer = new float[Ghosts.Length];
        IsScared = new bool[Ghosts.Length];
        IsDead = new bool[Ghosts.Length];
        deadCoroutine = new Coroutine[Ghosts.Length];
        GhostSpriteGO = new GameObject[Ghosts.Length];
        animatorController = new Animator[Ghosts.Length];
        spriteRenderer = new SpriteRenderer[Ghosts.Length];
        tween = new GhostTween[Ghosts.Length];
        previousDirection = new Direction[Ghosts.Length];
        currentDirection = new Direction[Ghosts.Length];
        ghostInitialDistance = new float[Ghosts.Length];

        for (int i = 0; i < Ghosts.Length; i++)
        {
            deadTimer[i] = 5.0f;
            currentDirection[i] = Direction.None;
            GhostSpriteGO[i] = Ghosts[i].transform.Find("Sprite").gameObject;
            animatorController[i] = GhostSpriteGO[i].GetComponent<Animator>();
            spriteRenderer[i] = GhostSpriteGO[i].GetComponent<SpriteRenderer>();
        }

        ghostOneInstructions = Ghosts[(int)Ghost.ONE].GetComponent<GhostOneInstructions>();
        ghostTwoInstructions = Ghosts[(int)Ghost.TWO].GetComponent<GhostTwoInstructions>();
        ghostThreeInstructions = Ghosts[(int)Ghost.THREE].GetComponent<GhostThreeInstructions>();
        ghostFourInstructions = Ghosts[(int)Ghost.FOUR].GetComponent<GhostFourInstructions>();
    }

    /// <summary>
    /// Handle the ghost movement
    /// </summary>
    private void handleMovement()
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            if (!IsDead[i])
            {
                if (IsScared[i])
                {
                    if (tween[i] == null)
                    {
                        if (!spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(Ghosts[i].transform.position)))
                        {
                            simulateScaredBehaviour(i);
                        }
                        else
                        {
                            handleGhostRoute(i);
                        }
                    }
                    else
                    {
                        handleLerping(i);
                    }
                }
                else if ((int)Ghost.ONE == i || (int)Ghost.TWO == i || (int)Ghost.THREE == i)
                {
                    if (tween[i] == null)
                    {
                        if (!spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(Ghosts[i].transform.position)))
                        {
                            findDirectionForGhost(i);
                        }
                        else
                        {
                            handleGhostRoute(i);
                        }
                    }
                    else
                    {
                        handleLerping(i);
                    }
                }
                else
                {
                    if (tween[i] == null)
                    {
                        handleGhostRoute(i);
                    }
                    else
                    {
                        handleLerping(i);
                    }
                }
            }
            else
            {
                handleLerping(i);
            }
        }
    }

    /// <summary>
    /// Simulate the behaviour of ghost1
    /// </summary>
    /// <param name="i"></param>
    private void simulateScaredBehaviour(int i)
    {
        string currentPosName = map.GetNameFromPosition(Ghosts[i].transform.position);

        if (numOfValidDirections(currentPosName) > 1)
        {
            previousDirection[i] = currentDirection[i];

            if (map.GetNameFromPosition(Ghosts[i].transform.position) == "TopLeft_14_0")
            {
                currentDirection[i] = Direction.Right;
            }
            else if (map.GetNameFromPosition(Ghosts[i].transform.position) == "TopRight_14_0")
            {
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

                checkDistanceOfGhost(i, currentPosName);
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
    /// check and keep current distance from pacStudent
    /// </summary>
    /// <param name="i"></param>
    /// <param name="currentPosName"></param>
    private void checkDistanceOfGhost(int i, string currentPosName)
    {
        float distanceFromGhostToPacStudent = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.transform.position);
        Vector3 DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[i]);
        float distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);

        if (distanceFromGhostToPacStudent < ghostInitialDistance[i] && distanceFromDestPosToPacStudent < distanceFromGhostToPacStudent)
        {
            Direction[] validDirections = getValidDirections(currentPosName);
            int randomPick;

            bool flag = false;
            Vector3 samplePos;
            float sampleDistance;

            foreach (Direction direction in validDirections)
            {
                samplePos = map.GetNeighbourPosition(currentPosName, direction);
                sampleDistance = Vector3.Distance(samplePos, pacStudentController.transform.position);

                if (sampleDistance > distanceFromGhostToPacStudent)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                while (distanceFromDestPosToPacStudent <= distanceFromGhostToPacStudent)
                {
                    randomPick = Random.Range(0, validDirections.Length);
                    currentDirection[i] = validDirections[randomPick];
                    DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[i]);
                    distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);
                }
            }
        }

        ghostInitialDistance[i] = distanceFromGhostToPacStudent;
    }

    /// <summary>
    /// Handle lerping of ghosts
    /// </summary>
    private void handleLerping(int i)
    {
        if (tween[i] != null)
        {
            Vector3 dest = IsDead[i] ? map.GetPositionFromName(ghostPositions[i]) : tween[i].DestPos;
            float distance = Vector3.Distance(Ghosts[i].transform.position, tween[i].DestPos);
            float durationTimer = IsDead[i] ? deadTimer[i] : duration;

            if (distance > marginalDistance)
            {
                Ghosts[i].transform.position = Vector3.Lerp(tween[i].StartPos, tween[i].DestPos, (Time.time - tween[i].StartTime) / durationTimer);
            }
            else
            {
                Ghosts[i].transform.position = tween[i].DestPos;
                tween[i] = null;
            }
        }
    }

    /// <summary>
    /// Set the route for ghost4
    /// </summary>
    private void handleGhostRoute(int i)
    {
        string currentPosName = map.GetNameFromPosition(Ghosts[i].transform.position);
        GhostInstruction instruction;

        switch (i)
        {
            case (int)Ghost.ONE:
                instruction = ghostOneInstructions.GetInstruction(currentPosName);
                break;
            case (int)Ghost.TWO:
                instruction = ghostTwoInstructions.GetInstruction(currentPosName);
                break;
            case (int)Ghost.THREE:
                instruction = ghostThreeInstructions.GetInstruction(currentPosName);
                break;
            case (int)Ghost.FOUR:
            default:
                instruction = ghostFourInstructions.GetInstruction(currentPosName);
                break;
        }

        if (instruction.PositionName != null)
        {
            currentDirection[i] = instruction.Direction;
        }

        Vector3 destPos = map.GetNeighbourPosition(currentPosName, currentDirection[i]);
        tween[i] = new GhostTween(Ghosts[i].transform.position, destPos, Time.time);
        handleRotateAndFlip(i);
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

                controlGhostOneMovement(currentPosName);
                controlGhostTwoMovement(currentPosName);
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
    /// Check if the distance between position the ghost is moving to and pacStudent is greater than the initial distance
    /// If condition is true, ghost can move freely, else, move away from pacStudent in a random direction
    /// </summary>
    private void controlGhostOneMovement(string currentPosName)
    {
        float distanceFromGhostToPacStudent = Vector3.Distance(Ghosts[(int)Ghost.ONE].transform.position, pacStudentController.transform.position);
        Vector3 DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[(int)Ghost.ONE]);
        float distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);

        if (distanceFromGhostToPacStudent < ghostInitialDistance[(int)Ghost.ONE] && distanceFromDestPosToPacStudent < distanceFromGhostToPacStudent)
        {
            Direction[] validDirections = getValidDirections(currentPosName);
            int randomPick;

            bool flag = false;
            Vector3 samplePos;
            float sampleDistance;

            foreach (Direction direction in validDirections)
            {
                samplePos = map.GetNeighbourPosition(currentPosName, direction);
                sampleDistance = Vector3.Distance(samplePos, pacStudentController.transform.position);

                if (sampleDistance > distanceFromGhostToPacStudent)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                while (distanceFromDestPosToPacStudent <= distanceFromGhostToPacStudent)
                {
                    randomPick = Random.Range(0, validDirections.Length);
                    currentDirection[(int)Ghost.ONE] = validDirections[randomPick];
                    DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[(int)Ghost.ONE]);
                    distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);
                }
            }
        }

        ghostInitialDistance[(int)Ghost.ONE] = distanceFromGhostToPacStudent;
    }

    /// <summary>
    /// Check if the distance between position the ghost is moving to and pacStudent is less than the initial distance
    /// Ghost will try to move closer towards PacStudent
    /// </summary>
    private void controlGhostTwoMovement(string currentPosName)
    {
        float distanceFromGhostToPacStudent = Vector3.Distance(Ghosts[(int)Ghost.TWO].transform.position, pacStudentController.transform.position);
        Vector3 DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[(int)Ghost.TWO]);
        float distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);

        if (distanceFromGhostToPacStudent > ghostInitialDistance[(int)Ghost.TWO] && distanceFromDestPosToPacStudent > distanceFromGhostToPacStudent)
        {
            Direction[] validDirections = getValidDirections(currentPosName);
            int randomPick;

            bool flag = false;
            Vector3 samplePos;
            float sampleDistance;

            foreach (Direction direction in validDirections)
            {
                samplePos = map.GetNeighbourPosition(currentPosName, direction);
                sampleDistance = Vector3.Distance(samplePos, pacStudentController.transform.position);

                if (sampleDistance < distanceFromGhostToPacStudent)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                while (distanceFromDestPosToPacStudent >= distanceFromGhostToPacStudent)
                {
                    randomPick = Random.Range(0, validDirections.Length);
                    currentDirection[(int)Ghost.TWO] = validDirections[randomPick];
                    DestPos = map.GetNeighbourPosition(currentPosName, currentDirection[(int)Ghost.TWO]);
                    distanceFromDestPosToPacStudent = Vector3.Distance(DestPos, pacStudentController.transform.position);
                }
            }
        }

        ghostInitialDistance[(int)Ghost.TWO] = distanceFromGhostToPacStudent;
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
    /// Return an array of valid directions
    /// </summary>
    /// <param name="posName"></param>
    /// <returns></returns>
    private Direction[] getValidDirections(string posName)
    {
        int counter = 0;
        Direction[] result = new Direction[numOfValidDirections(posName)];

        if (isDirectionValid(posName, Direction.Left))
        {
            result[counter] = Direction.Left;
            counter++;
        }

        if (isDirectionValid(posName, Direction.Right))
        {
            result[counter] = Direction.Right;
            counter++;
        }

        if (isDirectionValid(posName, Direction.Up) && posName != "BotLeft_11_13" && posName != "BotRight_11_13")
        {
            result[counter] = Direction.Up;
            counter++;
        }

        if (isDirectionValid(posName, Direction.Down) && posName != "TopLeft_11_13" && posName != "TopRight_11_13")
        {
            result[counter] = Direction.Down;
        }

        return result;
    }

    /// <summary>
    /// Return bool to determine whether direction is valid
    /// </summary>
    /// <param name="posName"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool isDirectionValid(string posName, Direction direction)
    {
        Vector3 position = map.GetNeighbourPosition(posName, direction);
        return position != Vector3.zero &&
            !spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(position)) &&
            !map.IsWallTypeFromPosition(position);
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
        IsDead[i] = false;

        if (i == (int)Ghost.ONE)
        {
            ghostInitialDistance[(int)Ghost.ONE] = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.gameObject.transform.position);
        }
        else if (i == (int)Ghost.TWO)
        {
            ghostInitialDistance[(int)Ghost.TWO] = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.gameObject.transform.position);
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
            ghostInitialDistance[i] = Vector3.Distance(Ghosts[i].transform.position, pacStudentController.gameObject.transform.position);
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
        bool inSpawn = false;
        animatorController[i].SetTrigger("DeadTrigger");

        if ((tween[i] != null &&
            (spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(tween[i].StartPos)) ||
            spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(tween[i].DestPos)))) ||
            spawnAreaPositions.IsInSpawn(map.GetNameFromPosition(Ghosts[i].transform.position)))
        {
            deadTimer[i] = 0.0f;
        }
        else
        {
            deadTimer[i] = 5.0f;
        }

        tween[i] = null;
        IsDead[i] = true;

        tween[i] = new GhostTween(Ghosts[i].transform.position, map.GetPositionFromName(ghostPositions[i]), Time.time);

        setMusicForDeadMode(inSpawn, i);

        yield return new WaitForSeconds(deadTimer[i]);

        setMusicForScaredMode(i);
        setAnimatorForGhost(i);

        IsDead[i] = false;

        yield return null;

        StopCoroutine(deadCoroutine[i]);
        deadCoroutine[i] = null;
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

    /// <summary>
    /// Check if at least 1 ghost is dead
    /// </summary>
    /// <returns></returns>
    private bool areAnyOtherGhostsDead(int i)
    {
        for (int j = 0; j < IsDead.Length; j++)
        {
            if (i != j && IsDead[j])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if at least 1 ghost is dead
    /// </summary>
    /// <returns></returns>
    private bool areAnyOtherGhostsScared(int i)
    {
        for (int j = 0; j < IsScared.Length; j++)
        {
            if (i != j && IsScared[j])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Handle music if in spawn, else play dead music
    /// </summary>
    /// <param name="inSpawn"></param>
    /// <param name="i"></param>
    private void setMusicForDeadMode(bool inSpawn, int i)
    {
        if (inSpawn)
        {
            if (!areAnyOtherGhostsDead(i))
            {
                if (HUDAspect.GhostTimerValue > 0)
                {
                    BackgroundMusic.playScaredMusic = true;
                    BackgroundMusic.playNormalMusic = false;
                }
                else
                {
                    BackgroundMusic.playNormalMusic = true;
                    BackgroundMusic.playScaredMusic = false;
                }
                BackgroundMusic.playDeadMusic = false;
            }
            else
            {
                BackgroundMusic.playDeadMusic = true;
                BackgroundMusic.playNormalMusic = false;
                BackgroundMusic.playScaredMusic = false;
            }
        }
        else
        {
            BackgroundMusic.playDeadMusic = true;
            BackgroundMusic.playNormalMusic = false;
            BackgroundMusic.playScaredMusic = false;
        }
    }

    /// <summary>
    /// Play the correct music depending on ghost states
    /// </summary>
    /// <param name="i"></param>
    private void setMusicForScaredMode(int i)
    {

        if (!areAnyOtherGhostsDead(i))
        {
            if (areAnyOtherGhostsScared(i))
            {
                BackgroundMusic.playScaredMusic = true;
                BackgroundMusic.playNormalMusic = false;
            }
            else
            {
                BackgroundMusic.playNormalMusic = true;
                BackgroundMusic.playScaredMusic = false;
            }
            BackgroundMusic.playDeadMusic = false;
        }
    }

    /// <summary>
    /// Set animator for ghosts
    /// </summary>
    /// <param name="i"></param>
    private void setAnimatorForGhost(int i)
    {
        if (IsScared[i])
        {
            if (HUDAspect.GhostTimerValue > 0 && HUDAspect.GhostTimerValue <= 3)
            {
                animatorController[i].SetTrigger("TransitionToRecoverTrigger");
            }
            else
            {
                animatorController[i].SetTrigger("ScaredTrigger");
            }
        }
        else
        {
            animatorController[i].SetTrigger("TransitionToNormalTrigger");
        }
    }
}