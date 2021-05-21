using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombParticles : MonoBehaviour
{
    [SerializeField] private float maxTime = 1;
    private float time;

    private ParticleSystem[] particle;
    private ParticleSystem.MainModule[] mainModulePart;

    private void Awake()
    {
        particle = GetComponentsInChildren<ParticleSystem>();
        mainModulePart = new ParticleSystem.MainModule[particle.Length];
        for (int i = 0; i < mainModulePart.Length; i++)
        {
            mainModulePart[i] = particle[i].main;
        }
    }

    private void Start()
    {
        float distance = (mainModulePart[0].startLifetime.constantMax * GameManager.Get.BombDistance());
        for (int i = 0; i < mainModulePart.Length; i++)
        {
            mainModulePart[i].startLifetime = distance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > maxTime)
            Destroy(gameObject);
    }
}
