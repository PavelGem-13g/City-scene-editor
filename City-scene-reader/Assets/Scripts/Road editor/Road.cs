using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Road : MonoBehaviour
{
    GameObject roadInList;
    Vector3 _realStartPosition;
    Vector3 _realEndPosition;
    float _width;
    float _hight;


    public void EditRoad(Vector3 realStartPosition, Vector3 realEndPosition, float width = 1, float hight = 1)
    {
        _realStartPosition = realStartPosition;
        _realEndPosition = realEndPosition;

        _width = width;
        _hight = hight;

        Vector3 startPosition = MapController.FromRealPosition(_realStartPosition);
        Vector3 endPosition = MapController.FromRealPosition(_realEndPosition);
        Vector3 lookAtPosition = new Vector3();

        List<GameObject> roadComponents = new List<GameObject>();
        transform.position = startPosition;

        float length = Vector3.Distance(startPosition, endPosition);
        float deltaLength = (length / RoadEditor.CountOfRoadComponents);
        float elementLength = deltaLength*1.5f;

        while (Vector3.Distance(startPosition, endPosition) > 0)//создание дороги
        {
            GameObject temp = Instantiate(RoadEditor.instance.roadCellPrefab, startPosition, Quaternion.identity, transform);
            
            temp.transform.position =
                new Vector3(temp.transform.position.x,
                MapController.instance.abstractMap.QueryElevationInUnityUnitsAt(MapController.instance.abstractMap.WorldToGeoPosition(new Vector3(temp.transform.position.x, 0, temp.transform.position.z))),
                temp.transform.position.z);

            lookAtPosition = Vector3.MoveTowards(startPosition, endPosition, deltaLength);

            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-width/2,0,0),
                new Vector3(width/2,0,0),
                new Vector3(width/2,_hight,0),
                new Vector3(-width/2,_hight,0),
                new Vector3(-width/2,_hight,elementLength),
                new Vector3(width/2,_hight,elementLength),
                new Vector3(width/2,0,elementLength),
                new Vector3(-width/2,0,elementLength),
            };

            int[] triangles = new int[]
            {
                0, 2, 1, //face front
			    0, 3, 2,
                2, 3, 4, //face top
			    2, 4, 5,
                1, 2, 5, //face right
			    1, 5, 6,
                0, 7, 4, //face left
			    0, 4, 3,
                5, 4, 7, //face back
			    5, 7, 6,
                0, 6, 7, //face bottom
			    0, 1, 6,
            };

            Mesh mesh = temp.GetComponent<MeshFilter>().mesh;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            roadComponents.Add(temp);

            temp.transform.LookAt(lookAtPosition);
            startPosition = lookAtPosition;
        }

        MeshCombiner meshCombiner = gameObject.AddComponent<MeshCombiner>();
        meshCombiner.CombineMeshes(false);
        Destroy(meshCombiner);


        foreach (var item in roadComponents)
        {
            item.Destroy();
        }
        roadComponents.Clear();
    }
    public Vector3 RealStartPosition
    {
        get
        {
            return _realStartPosition;
        }
    }
    public Vector3 RealEndPosition
    {
        get
        {
            return _realEndPosition;
        }
    }
    public void UpdateInformation()
    {
        EditRoad(_realStartPosition, _realEndPosition,_width,_hight);
    }
    public void CreadteRoadInList(GameObject parentOfRoadInList, GameObject prefab)
    {
        roadInList = Instantiate(prefab);
        roadInList.transform.parent = parentOfRoadInList.transform;
        roadInList.transform.localScale = Vector3.one;
        roadInList.GetComponentInChildren<Text>().text = name;
        roadInList.name = name;
    }
    public void Destroy()
    {
        Destroy(roadInList);
        Destroy(gameObject);
    }
}
