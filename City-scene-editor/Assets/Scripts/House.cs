using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject a;
    GameObject zxcqwe;
    int year;
    string houseName;
    [SerializeField] Vector3 xyz;

    public void Init(Vector3 Xyz, string NAME)
    {
        xyz = Xyz;
        houseName = NAME;
    }
    public string Name
    {
        get { return houseName; }
        set { houseName = value; }
    }
    public House(GameObject house)
    {
        zxcqwe = house;
    }
    public GameObject Obj
    {
        get { return zxcqwe; }
    }
    public int Year
    {
        get { return year; }
        set { year = value; }
    }
    
    public Vector3 Position
    {
        get { return xyz; }
        set { xyz = value; }
    }
}