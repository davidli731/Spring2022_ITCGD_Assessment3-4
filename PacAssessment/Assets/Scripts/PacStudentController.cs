using System.Collections;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private const float duration = 0.5f;
    private const float marginalDistance = 0.05f;

    private Animator animatorController;
    private SpriteRenderer pacStudentSpriteRenderer;
    private Map map;
    private PacStudentTween tween;
    private KeyCode lastInput;
    private KeyCode currentInput;
    private Direction currentDirection;
    private Coroutine ghostScaredCoroutine;
    private Coroutine pacStudentDeadCoroutine;

    [SerializeField] private GhostController ghostController;
    [SerializeField] private GameObject PacStudentSpriteGO;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioArray;
    [SerializeField] private ParticleSystem walkingParticleSystem;
    [SerializeField] private ParticleSystem deathParticleSystem;

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
        if (!isDead && !HUDAspect.IsStartTextActive && !HUDAspect.IsEndGame)
        {
            getPlayerInput();
        }

        // Handle Animations
        if (isWalking && !HUDAspect.IsEndGame)
        {
            playWalkingParticleEffect();

            animatorController.speed = 1;
            handleMovement();
        }
        else
        {
            stopWalkingParticleEffect();

            if (isDead)
            {
                animatorController.speed = 1;
            }
            else if (pacStudentSpriteRenderer.sprite == idleSprite)
            {
                animatorController.speed = 0;
            }
        }
    }

    /// <summary>
    /// Set initial position and direction
    /// </summary>
    public void Setup()
    {
        resetPacStudent();
        setInitPosition();
    }

    /// <summary>
    /// Get player input from WASD keys and set direction of movement
    /// </summary>
    private void getPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = KeyCode.W;
            isWalking = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = KeyCode.S;
            isWalking = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = KeyCode.A;
            isWalking = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = KeyCode.D;
            isWalking = true;
        }

        if ((Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D)) &&
            tween == null)
        {
            string posName = map.GetNameFromPosition(transform.position);
            currentDirection = getDirectionFromInput(lastInput);
            Vector3 destPos = map.GetNeighbourPosition(posName, currentDirection);

            if (destPos != Vector3.zero)
            {
                if (!map.IsWallTypeFromPosition(destPos))
                {
                    tween = new PacStudentTween(transform.position, destPos, Time.time);
                    handleRotateAndFlip();
                }
                else
                {
                    isWalking = false;
                }

                // if pacStudent is not walking, then it has hit a wall
                //playAudio(!isWalking);
            }
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
    /// Handle the player movement
    /// </summary>
    private void handleMovement()
    {
        if (tween != null)
        {
            float distance = Vector3.Distance(transform.position, tween.DestPos);

            if (distance > marginalDistance)
            {
                transform.position = Vector3.Lerp(tween.StartPos, tween.DestPos, (Time.time - tween.StartTime) / duration);
            } else
            {
                string currentPosName;
                Vector3 currentPos;
                Vector3 nextPos;

                currentDirection = getDirectionFromInput(lastInput);
                transform.position = tween.DestPos;

                teleport();

                currentPosName = map.GetNameFromPosition(transform.position);
                currentPos = map.GetPositionFromName(currentPosName);

                nextPos = map.GetNeighbourPosition(currentPosName, currentDirection);

                if (nextPos != Vector3.zero)
                {
                    if (!map.IsWallTypeFromPosition(nextPos))
                    {
                        // If next position in the given direction is walkable then create new tween
                        currentInput = lastInput;
                        tween = new PacStudentTween(currentPos, nextPos, Time.time);
                        handleRotateAndFlip();
                    }
                    else
                    {
                        if (currentInput != lastInput)
                        {
                            currentDirection = getDirectionFromInput(currentInput);
                            nextPos = map.GetNeighbourPosition(currentPosName, currentDirection);
                            if (nextPos != Vector3.zero && !map.IsWallTypeFromPosition(nextPos))
                            {
                                tween = new PacStudentTween(currentPos, nextPos, Time.time);
                                handleRotateAndFlip();
                            }
                            else
                            {
                                isWalking = false;
                            }
                        }
                        else
                        {
                            isWalking = false;
                        }
                    }

                    if (!isWalking)
                    {
                        Vector3 neighbourPosition = map.GetNeighbourPosition(map.GetNameFromPosition(transform.position), currentDirection);
                        ParticleSystem ps = map.GetWallParticleSystemFromPosition(neighbourPosition);
                        ps.Play();
                        audioSource.clip = audioArray[(int)PacStudentState.HitWall];
                        audioSource.Play();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Teleport if in middle left edge or middle right positions
    /// </summary>
    private void teleport()
    {
        if (transform.position == map.GetPositionFromName("TopLeft_14_0"))
        {
            currentDirection = Direction.Left;
            transform.position = map.GetPositionFromName("TopRight_14_0");
        }
        else if (transform.position == map.GetPositionFromName("TopRight_14_0"))
        {
            currentDirection = Direction.Right;
            transform.position = map.GetPositionFromName("TopLeft_14_0");
        }
    }

    /// <summary>
    /// Get player's current direction from their input 
    /// </summary>
    /// <param name="lastInput"></param>
    /// <returns></returns>
    private Direction getDirectionFromInput(KeyCode lastInput)
    {
        if (lastInput == KeyCode.W)
        {
            return Direction.Up;
        }
        else if (lastInput == KeyCode.S)
        {
            return Direction.Down;
        }
        else if (lastInput == KeyCode.A)
        {
            return Direction.Left;
        }
        else if (lastInput == KeyCode.D)
        {
            return Direction.Right;
        }
        return Direction.None;
    }

    /// <summary>
    /// Set pacStudent's initial starting position
    /// </summary>
    private void setInitPosition()
    {
        string startPosName = "TopLeft_1_1";
        Vector3 startPos = map.GetPositionFromName(startPosName);

        if (startPos != Vector3.zero)
        {
            transform.position = startPos;
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
        transform.rotation = Quaternion.identity;
        pacStudentSpriteRenderer.flipX = false;
        pacStudentSpriteRenderer.flipY = false;
    }

    /// <summary>
    /// Rotate and flip the pacStudent sprite given the direction
    /// </summary>
    private void handleRotateAndFlip()
    {
        resetPacStudent();

        if (currentDirection == Direction.Up)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
        }
        else if (currentDirection == Direction.Down)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
        }
        else if (currentDirection == Direction.Left)
        {
            pacStudentSpriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// Play walking particle system effect for pacStudent
    /// </summary>
    private void playWalkingParticleEffect()
    {
        if (currentDirection == Direction.Right ||
            currentDirection == Direction.Down ||
            currentDirection == Direction.Up)
        {
            walkingParticleSystem.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            walkingParticleSystem.transform.localScale = Vector3.one;
        }

        if (!walkingParticleSystem.isPlaying)
        {
            walkingParticleSystem.Play();
        }
    }

    /// <summary>
    /// Stop walking particle system effect for pacStudent
    /// </summary>
    private void stopWalkingParticleEffect()
    {
        if (walkingParticleSystem.isPlaying)
        {
            walkingParticleSystem.Stop();
        }
    }

    /// <summary>
    /// IEnumerator to start ghost scared state
    /// </summary>
    /// <returns></returns>
    private IEnumerator startGhostTimer()
    {
        int counter = (int)GhostController.ScaredTimer;
        HUDAspect.IsTimerActive = true;
        HUDAspect.GhostTimerValue = counter;
        BackgroundMusic.playScaredMusic = true;
        BackgroundMusic.playNormalMusic = false;
        BackgroundMusic.playDeadMusic = false;

        ghostController.TriggerScaredMode();

        for (int i = counter; i >= 0; i--)
        {
            yield return new WaitForSeconds(1.0f);
            HUDAspect.GhostTimerValue--;
        }

        HUDAspect.IsTimerActive = false;
        BackgroundMusic.playScaredMusic = false;
        BackgroundMusic.playDeadMusic = false;
        BackgroundMusic.playNormalMusic = true;

        StopCoroutine(ghostScaredCoroutine);
        ghostScaredCoroutine = null;
    }

    /// <summary>
    /// IEnumerator to respawn pacStudent after dead
    /// </summary>
    /// <returns></returns>
    private IEnumerator startPacStudentRespawn()
    {
        yield return new WaitForSeconds(2.0f);

        resetAndRespawn();

        StopCoroutine(pacStudentDeadCoroutine);
        pacStudentDeadCoroutine = null;
    }

    /// <summary>
    /// Reset and respawn pacStudent to the initial starting position
    /// </summary>
    private void resetAndRespawn()
    {
        Setup();
        isDead = false;
        pacStudentSpriteRenderer.sprite = idleSprite;
        animatorController.SetTrigger("NormalTrigger");
    }

    /// <summary>
    /// OnTriggerEnter collider method
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Ghost" && !isDead)
        {
            int ghostIndex = ghostController.GetIndexFromGameObject(collider.gameObject);

            if (!ghostController.IsScared[ghostIndex])
            {
                tween = null;
                isWalking = false;
                isDead = true;

                animatorController.SetTrigger("DeadTrigger");
                deathParticleSystem.Play();

                audioSource.clip = audioArray[(int)PacStudentState.Death];
                audioSource.Play();

                if (HUDAspect.LifeCount > 0)
                {
                    HUDAspect.LifeCount--;
                }

                if (HUDAspect.LifeCount > 0)
                {
                    if (pacStudentDeadCoroutine == null)
                    {
                        pacStudentDeadCoroutine = StartCoroutine(startPacStudentRespawn());
                    }
                }
                else
                {
                    endGame();
                }
            }
            else
            {
                ghostController.KillGhost(ghostIndex);
                HUDAspect.AddPoints(Points.Ghost);
            }
        }
        else if (collider.gameObject.tag == "BonusCherry")
        {
            audioSource.clip = audioArray[(int)PacStudentState.EatingPellet];
            audioSource.Play();
            Destroy(collider.gameObject);
            HUDAspect.AddPoints(Points.BonusCherry);
        }
        else
        {
            int mapIndex = map.GetIndexFromString(collider.gameObject.name);

            if (mapIndex >= 0)
            {
                if (map.mapItems[mapIndex].Type == Legend.StandardPellet ||
                    map.mapItems[mapIndex].Type == Legend.PowerPellet)
                {

                    if (map.mapItems[mapIndex].Type == Legend.StandardPellet)
                    {
                        HUDAspect.AddPoints(Points.StandardPellet);
                    }
                    else
                    {
                        Animator animator = map.mapItems[mapIndex].SpriteGO.GetComponent<Animator>();
                        animator.enabled = false;

                        if (ghostScaredCoroutine != null)
                        {
                            StopCoroutine(ghostScaredCoroutine);
                            ghostScaredCoroutine = null;
                        }

                        ghostScaredCoroutine = StartCoroutine(startGhostTimer());
                    }

                    SpriteRenderer spriteRenderer = map.mapItems[mapIndex].SpriteGO.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = null;
                    audioSource.clip = audioArray[(int)PacStudentState.EatingPellet];
                    audioSource.Play();
                    map.mapItems[mapIndex].Type = Legend.Empty;

                    LevelGenerator.NumOfPellets--;

                    if (LevelGenerator.NumOfPellets == 0)
                    {
                        endGame();
                    }
                }
                else if (map.mapItems[mapIndex].Type == Legend.Empty)
                {
                    audioSource.clip = audioArray[(int)PacStudentState.Walking];
                    audioSource.Play();
                }
            }
        }
    }

    /// <summary>
    /// All pellets eaten
    /// End Game here
    /// Note: end tween for ghosts as well
    /// </summary>
    private void endGame()
    {
        tween = null;
        isWalking = false;
        HUDAspect.IsEndGame = true;
    }
}