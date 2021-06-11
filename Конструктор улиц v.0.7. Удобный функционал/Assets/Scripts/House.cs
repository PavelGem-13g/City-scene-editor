using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject a;
    GameObject zxcqwe;
    int year;
    Vector3 xyz;

    public void Init(int Year, Vector3 Xyz)
    {
        year = Year;
        xyz = Xyz;
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