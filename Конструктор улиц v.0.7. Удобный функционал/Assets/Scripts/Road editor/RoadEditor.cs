using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadEditor : MonoBehaviour
{
    // Start is called before the first frame update
    public static RoadEditor instance;

    public static readonly int CountOfRoadComponents = 100;

    public GameObject roadPrefab;
    public GameObject roadCellPrefab;
    public List<Road> roads;

    public Image firstPositionButton;
    public Image secondPositionButton;

    public Vector3 startPosition;
    public Vector3 endPosition;

    public GameObject content;
    public GameObject roadInListPrefab;

    public GameObject changeNameOfStreet;

    int number;

    public InputField inputFieldWidth;
    public float width;
    public InputField inputFieldHight;
    public float hight;

    private void Awake()
    {
        instance = this;
        roads = new List<Road>();
        startPosition = Vector3.zero;
        endPosition = Vector3.zero;
        number = 0;
        /*firstPositionButton.color = Color.white;
        secondPositionButton.color = Color.white;*/
        width = 1;
        hight = 1;
    }
    private void Start()
    {
        transform.parent = MapController.instance.transform;
    }
    public void CreateRoad()
    {
        number++;
        BuildRoad(startPosition,endPosition, Convert.ToInt32(width), Convert.ToInt32(hight), number.ToString());
    }
    public void BuildRoad(Vector3 realStartPosition, Vector3 realEndPosition, int width = 1, int hight = 1, string name = "") 
    {
        GameObject temp = Instantiate(roadPrefab, transform);
        temp.name = name;
        Road tempRoad = temp.GetComponent<Road>();
        tempRoad.CreadteRoadInList(content, roadInListPrefab);
        tempRoad.EditRoad(realStartPosition, realEndPosition, width, hight);
        roads.Add(tempRoad);
        MyOwnClass.DifferentThings.Roads.Add(new MyOwnClass.SerializableRoad(realStartPosition,realEndPosition,width,hight,temp.name));
    }
    public void SetStartPosition()
    {
        // установка начльной позиции
        startPosition = FindObjectOfType<CameraFly>().gameObject.transform.position;
        if (startPosition == endPosition)
        {
            firstPositionButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            ClearColors();
        }
    }
    public void SetEndPosition()
    {
        //установка конечной позиции
        endPosition = FindObjectOfType<CameraFly>().gameObject.transform.position;
        if (startPosition == endPosition)
        {
            secondPositionButton.color = Color.red;
        }
        else
        {
            ClearColors();
        }
    }
    public void ClearColors()
    {
        firstPositionButton.color = Color.white;
        secondPositionButton.color = Color.white;
    }

    public void DeleteByName(string name)
    {
        foreach (var item in roads)
        {
            if (item.name == name)
            {
                //удаляем из списка дорог, а потом со сцены
                roads.Remove(item);
                Destroy(item);
                break;
            }
        }
    }
    public void ChangeName(string currentName, string newName)
    {
        foreach (var item in roads)
        {
            if (item.name == currentName)
            {
                item.name = newName;
                break;
            }
        }
        foreach (MyOwnClass.SerializableRoad item in MyOwnClass.DifferentThings.Roads)
        {
            if (item.name == currentName)
            {
                item.name = newName;
                break;
            }
        }
    }

    public void SetWidth()
    {
        width = (float)Convert.ToDouble(inputFieldWidth.text);
    }

    public void SetHight()
    {
        hight = (float)Convert.ToDouble(inputFieldHight.text);
    }

    public void ClearAllRoads() 
    {
        foreach (var item in roads)
        {
            item.Destroy();
        }
        roads.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}
