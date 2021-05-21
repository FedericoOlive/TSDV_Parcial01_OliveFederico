using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get { get => instance; }
    public PlayerController playerController;

    [Header("Bombs Settings")]
    [SerializeField] private int bombsDistance = 1;
    private float bombTimeExplosion = 1.5f;
    public int bombsCurrent;

    [Header("Player Settings")]
    

    [Header("Enemy Settings")]
    public int maxNormalEnemies;
    public int maxGhostEnemies;

    public int amountNormalEnemies;
    public int amountGhostEnemies;

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

    void Start()
    {

    }

    void Update()
    {
        
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

    
}
