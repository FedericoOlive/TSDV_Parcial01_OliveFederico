using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Get
    {
        get => instance;
    }

    public PlayerController playerController;
    public GameObject pfEnemy;

    [Header("Player Settings")] 
    [SerializeField] private float moveSpeedPlayer;
    [SerializeField] private float rotSpeedPlayer;
    [SerializeField] [Tooltip("Maximum simultaneous bombs")] private int bombsMax;
    [SerializeField] private int bombsDistance = 1;

    private float bombTimeExplosion = 1.5f;
    [NonSerialized] public int bombsCurrent;
    [SerializeField] private int playerProtectedSpawn;

    [Header("Normal Enemy Settings")] 
    [SerializeField] private int maxNormalEnemies;
    [SerializeField] private int speedMoveNormalEnemies;
    [SerializeField] private int speedRotNormalEnemies;
    [SerializeField] private LayerMask moveNormalBlockLayer;
    
    [Header("Ghost Enemy Settings")]
    [SerializeField] private int maxGhostEnemies;
    [SerializeField] private int speedMoveGhostEnemies;
    [SerializeField] private int speedRotGhostEnemies;
    [SerializeField] private LayerMask moveGhostBlockLayer;

    public int amountNormalEnemies;
    public int amountGhostEnemies;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [Header("Wall Destroyable Settings")]
    [SerializeField] private int totalDestWalls;
    [SerializeField] private int offsetProtectedSpawn;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        Enemy.enemyDestroyedEvent += DiminishEnemy;
    }

    // GETTERS:
    public int BombDistance()
    {
        return bombsDistance;
    }

    public float BombTimeExplosion()
    {
        return bombTimeExplosion;
    }

    public void GameStart()
    {
        WallsDestroyableStart();
        PlayerStart();
        EnemyStart();
    }

    void WallsDestroyableStart()
    {
        InstanceWall instanceWalls = GameObject.FindObjectOfType<InstanceWall>();
        instanceWalls.InstantiateAndSetPositionWallDest(totalDestWalls, offsetProtectedSpawn);
    }

    void PlayerStart()
    {
        if (!playerController)
            playerController = GameObject.FindObjectOfType<PlayerController>();

        playerController.SetPlayerSettings(moveSpeedPlayer, rotSpeedPlayer, bombsMax);
    }

    void EnemyStart()
    {
        Transform floor = GameObject.FindGameObjectWithTag("Floor").transform;
        Transform world = GameObject.Find("World").transform;
        amountNormalEnemies = maxNormalEnemies;
        amountGhostEnemies = maxGhostEnemies;
        for (int i = 0; i < maxNormalEnemies; i++)
        {
            SetEnemy(floor, world, Enemy.EnemyType.Normal, moveNormalBlockLayer, speedMoveNormalEnemies, speedRotNormalEnemies);
        }
        for (int i = 0; i < maxGhostEnemies; i++)
        {
            SetEnemy(floor, world, Enemy.EnemyType.Ghost, moveNormalBlockLayer, speedMoveGhostEnemies, speedRotGhostEnemies);
        }
    }

    void SetEnemy(Transform floor, Transform world, Enemy.EnemyType typeEnemy,LayerMask moveBlockLayer, float speedMove, float speedRot)
    {
        Vector3 size = floor.localScale;
       
        Vector3Int newPos = Vector3Int.zero;
        do
        {
            newPos.x = Random.Range(1, (int)size.x + 1);
            newPos.z = Random.Range(1, (int)size.z + 1);

        } while (CheckPosAvailable(newPos.x, newPos.z, (int)size.x));

        GameObject enemy = Instantiate(pfEnemy, newPos, Quaternion.identity, world);
        enemy.GetComponent<Enemy>().SetEnemy(typeEnemy, moveBlockLayer, speedMove, speedRot);
        enemy.transform.position *= 2;
        if (typeEnemy == Enemy.EnemyType.Ghost)
            enemy.layer = 14;

        enemies.Add(enemy);
    }


    bool CheckPosAvailable(int newX, int newZ, int max)
    {
        if (newX < playerProtectedSpawn && newZ < playerProtectedSpawn) 
            return true;

        for (int i = 1; i < max; i++) 
        {
            if (newX % 2 == 0 && newZ % 2 == 0)
            {
                return true;
            }
        }

        InstanceWall walls = GameObject.FindObjectOfType<InstanceWall>();
        for (int i = 0; i < walls.wallDest.Count; i++)
        {
            Vector3 pos = walls.wallDest[i].transform.localPosition;
            if (((int)pos.x) == newX && ((int)pos.z) == newZ)
            {
                return true;
            }
        }

        return false;
    }

    void DiminishEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemies.Count <= 0)
        {
            Debug.Log("Puerta Habilitada");
            // Habilitar Puerta
        }
    }
}