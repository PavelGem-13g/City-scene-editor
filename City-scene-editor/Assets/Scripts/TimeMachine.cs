using Mapbox.Utils;
using MyOwnClass;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using LitJson;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;

public class TimeMachine : MonoBehaviour
{
    string path;
    public TextMeshProUGUI sliderValue;
    public GameObject person;
    public static MainScript instance;

    [SerializeField] GameObject Спасоглинищевский9;
    [SerializeField] GameObject Лубянский15с2;
    [SerializeField] GameObject Лубянский19с1;
    [SerializeField] GameObject Лубянский21с5;
    [SerializeField] GameObject Лубянский25с2;
    [SerializeField] GameObject Лубянский271с1;
    [SerializeField] GameObject Маросейка215с1;
    [SerializeField] GameObject Маросейка42с1;
    [SerializeField] GameObject Маросейка68с1;
    [SerializeField] GameObject Маросейка8;
    [SerializeField] GameObject Покровский1610с1;
    [SerializeField] GameObject Покровский1815;
    [SerializeField] GameObject Покровский1618с44А;
    [SerializeField] GameObject Яузский10с2;
    [SerializeField] GameObject Спасоглинищевский3с1;
    [SerializeField] GameObject Спасоглинищевский91с10;
    [SerializeField] GameObject Спасоглинищевский91с16;
    [SerializeField] GameObject Спасоглинищевский12с5;

    int ExYear;
    string pathtoexcel;
    string ExName;
    float ExShirota;
    float ExDolgota;

    [SerializeField] List<House> Buildings = new List<House>();
    public void OK()
    {
        House house = Спасоглинищевский9.AddComponent<House>();
        house.Init(Vector3.zero, "Большой Спасоглинищевский пер, 91с7");
        house.gameObject.SetActive(false);
        Buildings.Add(house); //0

        House house1 = Лубянский15с2.AddComponent<House>();
        house1.Init(Vector3.zero, "Лубянский проезд, 15с2-4");
        house1.gameObject.SetActive(false);
        Buildings.Add(house1);
        AddToChange(Лубянский15с2); //1

        //house = Лубянский19с1.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Лубянский проезд, 19с1");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Лубянский19с1);

        //house = Лубянский21с5.AddComponent<House>();
        //house.Init(1820, Vector3.zero, "Лубянский проезд, 21с5");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Лубянский21с5);

        //house = Лубянский25с2.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Лубянский проезд, 25с2");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Лубянский25с2);

        //house = Лубянский271с1.AddComponent<House>();
        //house.Init(1850, Vector3.zero, "Лубянский проезд, 27/1с1");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Лубянский271с1);

        //house = Маросейка215с1.AddComponent<House>();
        //house.Init(1850, Vector3.zero, "Маросейка, ул., 2 / 15с1");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Маросейка215с1);

        //house = Маросейка42с1.AddComponent<House>();
        //house.Init(1850, Vector3.zero, "Маросейка, ул., 4 / 2с1");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Маросейка42с1);

        //house = Маросейка68с1.AddComponent<House>();
        //house.Init(1830, Vector3.zero, "Маросейка, ул., 6-8с1");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Маросейка68с1);


        //house = Маросейка8.AddComponent<House>();
        //house.Init(1830, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Маросейка8);

        //house = Покровский1610с1.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Покровский1610с1);

        //house = Покровский1815.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Покровский1815);

        //house = Покровский1618с44А.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Покровский1618с44А);

        //house = Яузский10с2.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Яузский10с2);

        //house = Спасоглинищевский3с1.AddComponent<House>();
        //house.Init(1820, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Спасоглинищевский3с1);

        //house = Спасоглинищевский91с10.AddComponent<House>();
        //house.Init(1840, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Спасоглинищевский91с10);

        //house = Спасоглинищевский91с16.AddComponent<House>();
        //house.Init(1840, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Спасоглинищевский91с16);

        //house = Спасоглинищевский12с5.AddComponent<House>();
        //house.Init(1860, Vector3.zero, "Маросейка, ул., 8");
        //house.gameObject.SetActive(false);
        //Buildings.Add(house);
        //AddToChange(Спасоглинищевский12с5);

    }
    public void Awake()
    {
        Buildings.Clear();
        Спасоглинищевский9 = MainScript.Load3dObjectByPathViaTriLib(@"C:\Users\79100\Downloads\City-scene-editor-main\3D_sorces\fbx\Большой Спасоглинищевский пер, 91с7.fbx");
        Лубянский15с2 = MainScript.Load3dObjectByPathViaTriLib(@"C:\Users\79100\Downloads\City-scene-editor-main\3D_sorces\fbx\Лубянский проезд, 15с2.obj");





    }
    public void OpenFromExcel()
    {
        GameObject obj;

        Excel xls = ExcelHelper.LoadExcel(pathtoexcel);

        for (int i = 2; i < xls.Tables[0].NumberOfRows; i++)
        {

            ExName = xls.Tables[0].GetCell(i, 1).Value.ToString();

            ExYear = Convert.ToInt32(xls.Tables[0].GetCell(i, 2).Value.ToString());
            ExShirota = float.Parse(xls.Tables[0].GetCell(i, 3).Value.ToString());
            ExDolgota = float.Parse(xls.Tables[0].GetCell(i, 4).Value.ToString());
            string pathToObj;
            pathToObj = pathtoexcel.Replace(Path.GetFileName(pathtoexcel), "") + ExName + ".obj";
            Debug.Log(pathToObj);
            obj = MainScript.Load3dObjectByPathViaTriLib(pathToObj);
            House house = obj.AddComponent<House>();
            house.gameObject.SetActive(false);
            house.Year = ExYear;
            AddToChange(obj);

            obj.transform.position = MapController.FromRealPosition(new Vector3((float)ExShirota, 300f, (float)ExDolgota));
            //house.gameObject.transform.position = house.Position;
            obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Buildings.Add(house);
            Debug.Log(i);

            //Buildings[i].Year = ExYear;
            //Buildings[i].Position = MapController.FromRealPosition(new Vector3(ExShirota, 300f, ExDolgota));
            //Buildings[i].gameObject.transform.position = Buildings[i].Position;
            //Debug.Log(Buildings[i].Name);
            //for (int j = 0; j < Buildings.Count; j++)
            //{
            //    if(ExName == Buildings[j].Name)
            //    {
            //        Buildings[j].Year = ExYear;
            //        Buildings[j].Position = MapController.FromRealPosition(new Vector3(ExShirota, 300f, ExDolgota));
            //    }
            //}

        }

    }
    public void AddToChange(GameObject usedObject)
    {

        //usedObject.transform.position = person.transform.position;
        MainScript.OptimizeGameObject(usedObject);
        DifferentThings.allRealObjects.Add(usedObject);

        DifferentThings.Objects.Add(new ObjectOfConstructor(
            MapController.GetRealPosition(usedObject.transform.position),
            usedObject.transform.rotation,
            usedObject.name,
            usedObject.transform.localScale.x,
            ""));
    }

    public void InstantiateOnData()
    {

        foreach (House h in Buildings)
        {
            h.gameObject.SetActive(false);
        }
        foreach (House h in Buildings)
        {

            if (int.Parse(sliderValue.text) >= h.Year)
            {
                h.gameObject.SetActive(true);
                //Instantiate(h.Obj, h.Position, Quaternion.identity);
            }
        }

    }
    public void Read()
    {
        string path = @"C:\Users\79100\Downloads\Здания ИГ в периметре с координатами (1).xlsx";
        Excel xls = ExcelHelper.LoadExcel(path);
        ExYear = int.Parse(xls.Tables[0].GetCell(2, 3).Value);
        Debug.Log(ExYear);

    }
    public void TimeMachineOn()
    {

        OpenFromExcel();
    }

    public void OpenPath()
    {
        StartCoroutine(LoadEverythingCoroutine());
    }
    IEnumerator LoadEverythingCoroutine()
    {
        FileBrowser.SetFilters(false, ".xlsx");
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Open project", "Open");
        if (FileBrowser.Success)
        {
            pathtoexcel = FileBrowser.Result[0];
            OpenFromExcel();
        }

    }
    public void TakeFromExcel()
    {
        //Buildings[0].gameObject.transform.position = new Vector3(5f, 5f, 5f);

        float x = 55.75723f;
        float y = 300;
        float z = 37.63227f;
        Vector3 temp = new Vector3(x, y, z);
        for (int j = 0; j < Buildings.Count; j++)
        {
            //Buildings[j].Position = MapController.FromRealPosition(temp);
            //Buildings[j].gameObject.transform.position = Buildings[j].Position;
            //for (int i = 0; i < 200; i++)
            //{
            //    _row = doc[i + 1];
            //    _cellName = _row[0];
            //    _cellYear = _row[1];
            //    _cellShirota = _row[2];
            //    _cellDolgota = _row[3];
            //    if (Buildings[j].Name == _cellName)
            //    {
            //        Buildings[j].Year = int.Parse(_cellYear);
            //        Buildings[j].Position = MapController.FromRealPosition(new Vector3(float.Parse(_cellShirota), 300f, float.Parse(_cellDolgota)));
            //        Buildings[j].gameObject.transform.position = Buildings[j].Position;
            //    }
            //}
        }

    }
}