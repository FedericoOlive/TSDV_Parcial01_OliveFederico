using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Get { get => instance; }

    public PlayerController playerController;
    [SerializeField] private int bombsDistance = 1;

    private float bombTimeExplosion = 1.5f;
    public int bombsCurrent;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
