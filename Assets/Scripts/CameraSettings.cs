using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public GameObject player;
    [Range(0, 20)] [SerializeField] private float spacingHight;
    [Range(0, -10)] [SerializeField] private float spacingFar;
    [Range(-2, 2)] [SerializeField] private float spacingOffset;
    private Vector3 offset;

    [SerializeField] private float velocity;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        offset = new Vector3(spacingOffset, spacingHight, spacingFar);

        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
