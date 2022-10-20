using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleSpeedController : MonoBehaviour
{
    private const float waitTimeBeforeSpawn = 10.0f;
    private const float boostTime = 5.0f;
    private const int xGridSize = 14;
    private const int yGridSize = 13;

    [SerializeField] private GameObject doubleSpeedGOPrefab;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private ExcludeRegions excludeRegions;

    private GameObject doubleSpeedGO;
    private Map map;
    private string[] sectors = { "TopLeft", "TopRight", "BotLeft", "BotRight" };
    private float time;
    private bool shouldSpawnDS;
    private bool gameStart;
    private Coroutine boostCoroutine;

    public static bool HasCollectedDoubleSpeed;
    public static bool HasUsedDoubleSpeed;

    private void Start()
    {
        HasCollectedDoubleSpeed = false;
        HasUsedDoubleSpeed = false;
        gameStart = true;
        shouldSpawnDS = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HUDAspect.IsStartTextActive)
        {
            createDSPellet();
        }
        else if (gameStart)
        {
            gameStart = false;

            time = Time.time;
            map = levelGenerator.GetMap();
        }

        if (Input.GetKeyDown(KeyCode.Space) && HasCollectedDoubleSpeed)
        {
            if (boostCoroutine == null)
            {
                boostCoroutine = StartCoroutine(boost());
            }
        }
    }

    /// <summary>
    /// Add double speed pellet to game
    /// </summary>
    private void createDSPellet()
    {
        if (shouldSpawnDS && Time.time - time >= waitTimeBeforeSpawn)
        {
            doubleSpeedGO = Instantiate(doubleSpeedGOPrefab, Vector3.zero, Quaternion.identity);

            string posName = sectors[Random.Range(0, sectors.Length)] + "_" +
                Random.Range(0, xGridSize + 1) + "_" +
                Random.Range(0, yGridSize + 1);

            while (map.GetLegendFromName(posName) != Legend.Empty || excludeRegions.IsIncluded(posName))
            {
                posName = sectors[Random.Range(0, sectors.Length)] + "_" +
                    Random.Range(0, xGridSize + 1) + "_" +
                    Random.Range(0, yGridSize + 1);
            }

            Vector3 position = map.GetPositionFromName(posName);

            doubleSpeedGO.transform.position = position;
            shouldSpawnDS = false;
        }
    }

    /// <summary>
    /// Boost player speed
    /// </summary>
    /// <returns></returns>
    private IEnumerator boost()
    {
        HasUsedDoubleSpeed = true;
        PacStudentController.PlayerSpeed = 2.0f;
        yield return new WaitForSeconds(boostTime);
        PacStudentController.PlayerSpeed = 1.0f;

        HasCollectedDoubleSpeed = false;
        HasUsedDoubleSpeed = false;
        shouldSpawnDS = true;
        time = Time.time;

        StopCoroutine(boostCoroutine);
        boostCoroutine = null;
    }
}
