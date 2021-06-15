using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSettingsController : MonoBehaviour
{
    [SerializeField] Camera camera;


    [SerializeField] Slider cameraZoomSlider;
    [SerializeField] Dropdown projectionsDropdown;
    [SerializeField] Dropdown skyboxDropdown;

    [SerializeField] Material defaultMaterial;
    [SerializeField] Material customMaterial;

    public void ChangeCameraZoom()
    {
        camera.fieldOfView = cameraZoomSlider.value;
        camera.orthographicSize = cameraZoomSlider.value;
    }

    public void ChangePerspective() 
    {
        if (projectionsDropdown.value==0)
        {
            camera.orthographic = false;
        }
        else 
        {
            camera.orthographic = true;
        }
    }

    public void ChangeSkyBox() 
    {
        if (skyboxDropdown.value == 0)
        {
            RenderSettings.skybox = defaultMaterial;
        }
        else 
        {
            RenderSettings.skybox = customMaterial;
        }
    }
}
