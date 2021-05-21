﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Bomb : MonoBehaviour
{
    public GameObject pfParticleExplosion;
    [SerializeField] private LayerMask layersDamagable;
    [SerializeField] private LayerMask layersBlocked;
    [SerializeField] private int damage;

    private float timeExplosion;
    private float time;
    private bool explode;
    private float timeToExplode = 0.5f;
    enum Directions { Forward, Right, Back, Left }
    private Directions direction = Directions.Forward;
    private Vector3[] endPos = new Vector3[4];
    private bool[] enabledPos = new bool[4];

    private List<GameObject> damaged = new List<GameObject>();
    private Renderer rend;
    private Color startColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        GameManager.Get.bombsCurrent++;
    }

    void Start()
    {
        for (int i = 0; i < enabledPos.Length; i++)
        {
            enabledPos[i] = true;
        }

        timeExplosion = GameManager.Get.BombTimeExplosion();
        startColor = rend.material.color;
    }

    void Update()
    {
        time += Time.deltaTime; 
        rend.material.color = Color.Lerp(startColor, Color.red, time / timeExplosion);
        if (time > timeExplosion && !explode)
        {
            explode = true;
            Explode();
            Destroy(gameObject, timeToExplode);
        }
    }

    private void OnDestroy()
    {
        Instantiate(pfParticleExplosion, transform.position, Quaternion.identity);
    }

    void Explode()
    {
        int length = GameManager.Get.BombDistance();
        for (int i = 0; i < length; i++)
        {
            for (int k = 0; k < endPos.Length; k++)
            {
                if (enabledPos[k])
                    CheckFourDirections(k, (i + 1) * 2);
            }
        }
    }

    void CheckFourDirections(int dir, int length)
    {
        Vector3 vectorDirection=Vector3.zero;

        direction = (Directions)dir;
        switch (direction)
        {
            case Directions.Forward:
                vectorDirection = Vector3.forward;
                break;
            case Directions.Right:
                vectorDirection = Vector3.right;
                break;
            case Directions.Back:
                vectorDirection = Vector3.back;
                break;
            case Directions.Left:
                vectorDirection = Vector3.left;
                break;
        }

        RaycastHit[] hit = Physics.RaycastAll(transform.position, vectorDirection, length);

        foreach (RaycastHit hitted in hit)
        {
            if (LayerEquals(layersDamagable, hitted.transform.gameObject.layer))
            {
                Debug.Log("La bomba dañó a: " + hitted.transform.name);
            }
            
            if (LayerEquals(layersBlocked, hitted.transform.gameObject.layer))
            {
                enabledPos[dir] = false;
            }
        }
    }

    bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
