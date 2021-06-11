using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.GroundEditor;
using Mapbox.Unity.Utilities;

public class GroundEditor : MonoBehaviour
{
    public static GroundEditor instance;
    public GameObject markersParent;
    public GameObject markersPrefab;

    public List<Mark> markers;
    int number;

    private Mark _startMarker = null;
    private Mark _endMarker = null;

    public GameObject linesParent;
    [SerializeField] private GameObject linePrefab;
    public List<Line> lines;

    public GameObject map;
    [SerializeField] private GameObject vertexPrefab;
    public List<Vertex> vertices;

    [SerializeField] int sizeX = 800;
    [SerializeField] int sizeY = 800;

    [SerializeField] Text longitude;
    [SerializeField] Text latitude;

    [SerializeField] InputField width;
    [SerializeField] InputField height;

    private void Awake()
    {
        markers = new List<Mark>();
        instance = this;
        number = 0;
    }

    public void SetMarker(Mark mark) 
    {
        if (_startMarker == null)
        {
            _startMarker = mark;
        }
        else 
        {
            _endMarker = mark;
            DrawLine();
            ClearMarks();
        }
    }

    void ClearMarks() 
    {
        _startMarker = null;
        _endMarker = null;
    }

    void DrawLine() 
    {
        GameObject temp = Instantiate(linePrefab);
        temp.transform.parent = linesParent.transform;
        Line line = temp.GetComponent<Line>();
        line.SetLine(_startMarker, _endMarker);
        line.DrawLine();
        lines.Add(line);
    }
    public void UpdateLines() 
    {
        foreach (var item in lines)
        {
            item.DrawLine();
        }
    }

    public void NewMarker()
    {
        GameObject temp = Instantiate(markersPrefab);
        temp.transform.parent = markersParent.transform;
        Mark mark = temp.GetComponent<Mark>();
        mark.rectTransform.transform.localPosition = Vector3.zero;
        markers.Add(mark);
    }
    public void DeleteMarker(Mark mark) 
    {
        markers.Remove(mark);
        for (int i = lines.Count-1; i >=0; i--)
        {
            if (lines[i].IsContainsMarker(mark))
            {
                lines[i].Delete();
            }
        }
    }

    public void UpdateRealPosition(Vector3 position) 
    {
        Vector3 bounds = MapController.instance.TerrainSize;
        Vector3 temp = new Vector3((bounds.x * position.x / sizeX),
                    0f,
                    bounds.y * position.y / sizeY);
        Vector3 converted = MapController.GetRealPosition(temp);
        longitude.text = "Longitude: "+ converted.x.ToString();
        latitude.text = "Latitude: "+converted.z.ToString();
    }

    public void BuildStreetNet() 
    {
        RoadEditor.instance.ClearAllRoads();

        Vector3 bounds = MapController.instance.TerrainSize;
        float tempScaleY = MapController.instance.abstractMap.gameObject.transform.localScale.y;
        float tempScaleX = MapController.instance.abstractMap.gameObject.transform.localScale.x;

        MapController.instance.abstractMap.gameObject.transform.localScale = new Vector3(tempScaleX, tempScaleX, tempScaleX);

        foreach (var item in lines)
        {
            RoadEditor.instance.BuildRoad(
                MapController.GetRealPosition( new Vector3(
                    (bounds.x * item.StartPosition.x / sizeX),
                    MapController.instance.abstractMap.QueryElevationInUnityUnitsAt(MapController.instance.abstractMap.WorldToGeoPosition(new Vector3((bounds.x * item.StartPosition.x / sizeX), 0, (bounds.y * item.StartPosition.y / sizeY)))),
                    bounds.y * item.StartPosition.y / sizeY)),
                 MapController.GetRealPosition( new Vector3(
                    (bounds.x * item.EndPosition.x / sizeX),
                    MapController.instance.abstractMap.QueryElevationInUnityUnitsAt(MapController.instance.abstractMap.WorldToGeoPosition(new Vector3(bounds.x * item.EndPosition.x / sizeX, 0, bounds.y * item.EndPosition.y / sizeY))),
                    bounds.y * item.EndPosition.y / sizeY)),
                Convert.ToInt32(width.text),
                Convert.ToInt32(height.text));
        }

        MapController.instance.abstractMap.gameObject.transform.localScale = new Vector3(tempScaleX, tempScaleY, tempScaleX);
    }
}
