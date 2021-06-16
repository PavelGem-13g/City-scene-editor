using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject a;
    GameObject zxcqwe;
    int year;
    string name;
    [SerializeField] Vector3 xyz;

    public void Init(Vector3 Xyz, string NAME)
    {
        xyz = Xyz;
        name = NAME;
    }
    public string Name
    {
        get { return name; }
        set { name = value; }
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