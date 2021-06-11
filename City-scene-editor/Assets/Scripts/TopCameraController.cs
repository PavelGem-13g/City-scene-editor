using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TopCameraController : MonoBehaviour
{
    TopCameraController _instance;
    Camera _topCamera;
    [SerializeField] private float maxZoom = 1;
    [SerializeField] private float minZoom = 76;
    [SerializeField] private float zoomLimiter = 1900;

    public TopCameraController Instance 
    {
        get 
        {
            return _instance;
        }
    }

    public float Scale 
    {
        get
        {
            return GetGreatestDistance() / zoomLimiter;
        }
    }

    private void Awake()
    {
        _instance = this;
        _topCamera = GetComponent<Camera>();
    }

    void Update()
    {
        _topCamera.fieldOfView =  (minZoom/maxZoom)*Scale;
    }
    float GetGreatestDistance()
    {
        if (MapController.instance.TerrainSize.x> MapController.instance.TerrainSize.y)
        {
            return MapController.instance.TerrainSize.x;
        }
        else 
        {
            return MapController.instance.TerrainSize.y;
        }
    }
}
