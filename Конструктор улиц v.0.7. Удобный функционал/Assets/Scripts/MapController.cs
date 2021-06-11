using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using MyOwnClass;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public AbstractMap abstractMap;

    public static MapController instance;

    [SerializeField] InputField latitude;
    [SerializeField] InputField longitude;
    [SerializeField] Slider zoom;

    [SerializeField] Slider west;
    [SerializeField] Slider north;
    [SerializeField] Slider east;
    [SerializeField] Slider south;
    [SerializeField] Slider y;

    [SerializeField] Texture2D grassTexture;
    [SerializeField] Slider textureSlider;

    public List<UnityTile> Tiles 
    {
        get 
        {
            List<UnityTile> unityTiles = new List<UnityTile>();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<UnityTile>())
                {
                    unityTiles.Add(transform.GetChild(i).GetComponent<UnityTile>());
                }
            }
            return unityTiles;
        }
    }

    public Vector3 TerrainSize 
    {
        get 
        {
            RangeTileProviderOptions options = abstractMap.Options.extentOptions.defaultExtents.rangeAroundCenterOptions;
            return new Vector3(1+options.east+options.west,1+options.south+options.north)*100f;
        }
    }

    private void Awake()
    {
        instance = this;
        abstractMap = GetComponent<AbstractMap>();
    }
    private void Start()
    {
        SetTexture();
        // RebuildMap();
    }

    public void RebuildMap()
    {
        if (latitude.text.Length > 0 && longitude.text.Length > 0 && latitude.text[latitude.text.Length - 1] != '.' && longitude.text[longitude.text.Length - 1] != '.')
        {
            abstractMap.UpdateMap(Conversions.StringToLatLon(latitude.text+","+longitude.text), Mathf.RoundToInt(zoom.value));

            abstractMap.Options.extentOptions.
                defaultExtents.rangeAroundCenterOptions.west = Convert.ToInt32(west.value);
            abstractMap.Options.extentOptions.
                defaultExtents.rangeAroundCenterOptions.north = Convert.ToInt32(north.value);
            abstractMap.Options.extentOptions.
                defaultExtents.rangeAroundCenterOptions.east = Convert.ToInt32(east.value);
            abstractMap.Options.extentOptions.
                defaultExtents.rangeAroundCenterOptions.south = Convert.ToInt32(south.value);
        }

        for (int i = 0; i < DifferentThings.allRealObjects.Count; i++)
        {
            if (IsOnTerrain(DifferentThings.Objects[i].position.ToVector3()))
            {
                DifferentThings.allRealObjects[i].transform.position = FromRealPosition(DifferentThings.Objects[i].position.ToVector3());
                DifferentThings.allRealObjects[i].SetActive(true);
            }
            else 
            { 
                DifferentThings.allRealObjects[i].SetActive(false);
            }
        }
        foreach (var item in RoadEditor.instance.roads)
        {
            if (IsOnTerrain(item.RealStartPosition) && IsOnTerrain(item.RealEndPosition))
            {
                item.gameObject.SetActive(true);
                item.UpdateInformation();
            }
            else 
            {
                item.gameObject.SetActive(false);
            }
        }
        ResizeMap();
    }
    public void ResizeMap()
    {
        transform.localScale = new Vector3(transform.localScale.x, y.value * transform.localScale.x, transform.localScale.z);
    }
    public static float YScale
    {
        get
        {
            return instance.y.value;
        }
    }
    public void SetTexture()
    {
        if (textureSlider.value == 1)
        {
            try 
            {
            abstractMap.ImageLayer.SetLayerSource(ImagerySourceType.None);   
            }
            catch 
            {
            
            }
            foreach (var item in Tiles)
            {
                item.GetComponent<MeshRenderer>().material.mainTexture = grassTexture;
            }
        }
        else
        {
            abstractMap.ImageLayer.SetLayerSource(ImagerySourceType.MapboxSatellite);
        }
    }
    public static Vector3 GetRealPosition(Vector3 unityPosition)
    {
        Vector2d position = instance.abstractMap.WorldToGeoPosition(unityPosition);
        float height = (instance.abstractMap.QueryElevationInMetersAt(position) *
                    (unityPosition.y / (instance.abstractMap.QueryElevationInUnityUnitsAt(position) * YScale)));
        return new Vector3((float)position.x, height, (float)position.y);
    }
    public static Vector3 FromRealPosition(Vector3 realPosition)
    {
        Vector3 temp = instance.abstractMap.GeoToWorldPosition(new Vector2d(realPosition.x, realPosition.z), false);
        temp.y = (float)instance.abstractMap.QueryElevationInUnityUnitsAt(new Vector2d(realPosition.x, realPosition.z)) * YScale *
            (realPosition.y /
                 instance.abstractMap.QueryElevationInMetersAt(new Vector2d(realPosition.x, realPosition.z)));
        return temp;
    }
    public static bool IsOnTerrain(Vector3 realPosition)
    {
        return !float.IsNaN((float)instance.abstractMap.QueryElevationInUnityUnitsAt(new Vector2d(realPosition.x, realPosition.z)) * YScale *
            (realPosition.y /
                 instance.abstractMap.QueryElevationInMetersAt(new Vector2d(realPosition.x, realPosition.z))));
    }
}
