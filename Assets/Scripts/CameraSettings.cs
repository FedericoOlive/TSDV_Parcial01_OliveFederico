using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    enum CameraMode { Perspective, Orthographic }

    public GameObject player;
    public GameObject floor;
    [SerializeField] bool playerDependence = true;
    [Range(0, 40)] [SerializeField] private float spacingHight;
    [Range(0, -10)] [SerializeField] private float spacingFar;
    [Range(-2, 2)] [SerializeField] private float spacingOffset;
    [SerializeField] private CameraMode cameraMode;
    [Range(5, 20)] [SerializeField] private int size;
    private CameraMode lastCameraMode;
    private Camera cam;
    private Vector3 offset;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        lastCameraMode = cameraMode;
    }

    void Update()
    {
        SetPositionAndRotation();

        if (lastCameraMode != cameraMode)
        {
            SetCameraMode();
        }
    }

    void SetPositionAndRotation()
    {
        if (playerDependence)
        {
            offset = new Vector3(spacingOffset, spacingHight, spacingFar);

            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform);
        }
        else
        {
            offset = new Vector3(spacingOffset, spacingHight, spacingFar);

            transform.position = floor.transform.position + offset;
            transform.LookAt(floor.transform);
        }
        
        cam.orthographicSize = size;
    }

    void SetCameraMode()
    {
        lastCameraMode = cameraMode;
        switch (cameraMode)
        {
            case CameraMode.Perspective:
                cam.orthographic = false;
                break;
            case CameraMode.Orthographic:
                cam.orthographic = true;
                break;
        }
    }
}
