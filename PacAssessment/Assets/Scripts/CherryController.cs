using UnityEngine;

public class CherryController : MonoBehaviour
{
    private const float waitUntilSpawn = 10.0f;
    private const float durationOfTravel = 10.0f;

    [SerializeField] private GameObject bonusGOPrefab;

    private bool spawnBonus = false;
    private float spawnTime = 0.0f;
    private Direction directionOfSpawn;
    private Direction directionOfTravel;
    private Vector3 startPos;
    private Vector3 endPos;
    private DemoTween bonusTween;
    private GameObject bonusGO;

    // Update is called once per frame
    void Update()
    {
        if (!HUDAspect.IsStartTextActive)
        {
            createBonus();
            handleMovement();
        }
    }

    /// <summary>
    /// Create bonus every 10 seconds
    /// </summary>
    private void createBonus()
    {
        if ((Time.time < waitUntilSpawn || Time.time - spawnTime >= waitUntilSpawn) && !spawnBonus)
        {
            spawnTime = Time.time;
            spawnBonus = true;
            directionOfSpawn = (Direction)Random.Range(0, 4);

            if (directionOfSpawn == Direction.Up)
            {
                directionOfTravel = Direction.Down;
                startPos = new Vector3(0.0f, 10.2f, 0.0f);
                endPos = new Vector3(0.0f, -10.2f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Down)
            {
                directionOfTravel = Direction.Up;
                startPos = new Vector3(0.0f, -10.2f, 0.0f);
                endPos = new Vector3(0.0f, 10.2f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Left)
            {
                directionOfTravel = Direction.Right;
                startPos = new Vector3(18.0f, 0.0f, 0.0f);
                endPos = new Vector3(-18.0f, 0.0f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Right)
            {
                directionOfTravel = Direction.Left;
                startPos = new Vector3(-18.0f, 0.0f, 0.0f);
                endPos = new Vector3(18.0f, 0.0f, 0.0f);
            }

            bonusGO = Instantiate(bonusGOPrefab, Vector3.zero, Quaternion.identity);
            bonusGO.transform.position = startPos;

            if (bonusTween == null)
            {
                bonusTween = new DemoTween(bonusGO.transform.position, endPos, Time.timeSinceLevelLoad, durationOfTravel, directionOfTravel);
            }
        }
    }

    /// <summary>
    /// Handle object movement tweening
    /// </summary>
    private void handleMovement()
    {
        if (bonusGO != null)
        {
            if (bonusTween != null)
            {
                if (bonusGO.transform.position != endPos)
                {
                    bonusGO.transform.position = Vector3.Lerp(bonusTween.StartPos, bonusTween.DestPos, (Time.timeSinceLevelLoad - bonusTween.StartTime) / durationOfTravel);
                }
                else
                {
                    spawnBonus = false;
                    Destroy(bonusGO);
                    bonusTween = null;
                }
            }
        }
        else
        {
            spawnBonus = false;
            bonusTween = null;
        }
    }
}
