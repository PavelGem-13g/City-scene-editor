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
    public static TimeMachine instance;
    string path;
    public TextMeshProUGUI sliderValue;
    public GameObject person;

    int ExYear;
    string pathtoexcel;
    string ExName;
    float ExShirota;
    float ExHight;
    float ExDolgota;
    int ExMaterial;
    float ExRotation1;
    float ExRotation2;
    float ExRotation3;
    public List<House> Houses = new List<House>();

    public Material one;
    public Material two;
    public Material three;
    public Material four;
    public Material five;
    public Material six;
    public Material seven;

    public void Awake()
    {
        instance = this;
    }
    public void OpenFromExcel()
    {
        GameObject obj;

        Excel xls = ExcelHelper.LoadExcel(pathtoexcel);

        for (int i = 2; i <= xls.Tables[0].NumberOfRows; i++)
        {

            ExName = xls.Tables[0].GetCell(i, 1).Value.ToString();
            ExYear = Convert.ToInt32(xls.Tables[0].GetCell(i, 2).Value.ToString());
            ExShirota = float.Parse(xls.Tables[0].GetCell(i, 3).Value.ToString());
            ExDolgota = float.Parse(xls.Tables[0].GetCell(i, 4).Value.ToString());
            ExMaterial = Convert.ToInt32(xls.Tables[0].GetCell(i, 5).Value.ToString());
            ExHight = float.Parse(xls.Tables[0].GetCell(i, 6).Value.ToString());
            ExRotation1 = float.Parse(xls.Tables[0].GetCell(i, 7).Value.ToString());
            ExRotation2 = float.Parse(xls.Tables[0].GetCell(i, 8).Value.ToString());
            ExRotation3 = float.Parse(xls.Tables[0].GetCell(i, 9).Value.ToString());

            string pathToObj = pathtoexcel.Replace(Path.GetFileName(pathtoexcel), "") + ExName + ".obj";
            Debug.Log(pathToObj);

            Directory.CreateDirectory(Application.streamingAssetsPath + "/Streaming/");
            File.Copy(pathToObj, Application.streamingAssetsPath + "/Streaming/" + Path.GetFileName(pathToObj), true);
            if (File.Exists(pathToObj.Replace(Path.GetExtension(pathToObj), ".mtl")))
            {
                File.Copy(pathToObj.Replace(Path.GetExtension(pathToObj), ".mtl"), Application.streamingAssetsPath + "/Streaming/" + Path.GetFileName(pathToObj.Replace(Path.GetExtension(pathToObj), ".mtl")), true);
            }

            obj = MainScript.Load3dObjectByPathViaTriLib(pathToObj);
            MainScript.OptimizeGameObject(obj);
            House house = obj.AddComponent<House>();
            MeshRenderer rd = obj.transform.GetChild(0).GetComponent<MeshRenderer>();

            switch (ExMaterial)
            {
                case 0:
                    break;
                case 1:
                    rd.material = one;
                    break;
                case 2:
                    rd.material = two;
                    break;
                case 3:
                    rd.material = three;
                    break;
                case 4:
                    rd.material = four;
                    break;
                case 5:
                    rd.material = five;
                    break;
                case 6:
                    rd.material = six;
                    break;
                case 7:
                    rd.material = seven;
                    break;
            }
            house.gameObject.SetActive(false);
            house.Year = ExYear;


            obj.transform.position = MapController.FromRealPosition(new Vector3((float)ExShirota, ExHight, (float)ExDolgota));
            obj.transform.Rotate(ExRotation1, ExRotation2, ExRotation3, Space.Self); ;
            obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            DifferentThings.allRealObjects.Add(obj);

            DifferentThings.Objects.Add(new ObjectOfConstructor(
                MapController.GetRealPosition(obj.transform.position),
                obj.transform.rotation,
                Path.GetFileName(pathToObj),
                obj.transform.localScale.x,
                "",
                house.Year,
                ExMaterial));
            Houses.Add(house);
            Debug.Log(i);
        }

    }

    public void InstantiateOnData()
    {

        foreach (House h in Houses)
        {
            h.gameObject.SetActive(false);
        }
        foreach (House h in Houses)
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
        for (int j = 0; j < Houses.Count; j++)
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