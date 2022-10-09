using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    private const float waitUntilSpawn = 10.0f;
    private const float durationOfTravel = 5.0f;

    [SerializeField] private GameObject bonusGO;

    private bool spawnBonus = false;
    private Direction directionOfSpawn;
    private Direction directionOfTravel;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad % waitUntilSpawn >= 0.0f && !spawnBonus)
        {
            spawnBonus = true;
            directionOfSpawn = (Direction)Random.Range(0, 4);

            if (directionOfSpawn == Direction.Up)
            {
                directionOfTravel = Direction.Down;
                startPos = new Vector3(0.0f, 230.0f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Down)
            {
                directionOfTravel = Direction.Up;
                startPos = new Vector3(0.0f, -230.0f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Left)
            {
                directionOfTravel = Direction.Right;
                startPos = new Vector3(410.0f, 0.0f, 0.0f);
            }
            else if (directionOfSpawn == Direction.Right)
            {
                directionOfTravel = Direction.Left;
                startPos = new Vector3(-410.0f, 0.0f, 0.0f);
            }
            //Instantiate(bonusGO, )
        }
    }
}
