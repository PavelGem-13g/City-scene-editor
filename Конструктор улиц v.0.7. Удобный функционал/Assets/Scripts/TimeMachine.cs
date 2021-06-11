using System.Collections.Generic;
using UnityEngine;
using FlexFramework.Excel;
using TMPro;
using SimpleFileBrowser;
using System.Collections;

public class TimeMachine : MonoBehaviour
{
    string path;
    Document doc;
    public TextMeshProUGUI sliderValue;

    [SerializeField]GameObject Спасоглинищевский9;

    [SerializeField]List<House> Buildings = new List<House>();
    public void Awake()
    {
        Buildings.Clear();
        Спасоглинищевский9 = MainScript.Load3dObjectByPathViaTriLib(@"C:\Users\pashe\Documents\GitHub\street_designer\3D_sorces\fbx\Большой Спасоглинищевский пер, 91с7.fbx");
        //House Спасоглин9 = new House(Спасоглинищевский9);
        House house = Спасоглинищевский9.AddComponent<House>();
        house.Init(1700, Vector3.zero);
        Buildings.Add(house);
    }
    public void InstantiateOnData()
    {
        
        foreach  (House h in Buildings)
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
    public void OpenPath()
    {
        StartCoroutine(LoadEverythingCoroutine());
    }
    IEnumerator LoadEverythingCoroutine()
    {
        FileBrowser.SetFilters(false, ".xlsx");
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Open project", "Open");
        if(FileBrowser.Success)
        {
            path = FileBrowser.Result[0];
        }
        doc = Document.LoadAt(path);
    }
    public void TakeFromExcel()
    {
        
        Row _row;
        Cell _cell, _cell2, _cell3;
        //Excel.Application ObjExcel = new Excel.Application();

        //Excel.Workbook ObjWorkBook;
        //Excel.Worksheet ObjWorkSheet;
        //ObjWorkBook = ObjExcel.Workbooks.Open(path);
        //ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
        for (int i = 0; i < 1; i++)
        {

            _row = doc[i+1];
            _cell3 = _row[1];
            _cell = _row[2];
            _cell2 = _row[3];
            Vector3 temp = new Vector3(_cell, _cell2, 300f);
            Buildings[i].Init(_cell3, MapController.FromRealPosition(temp));
            //Buildings[i].Year = _cell3;
            //Buildings[i].Position = MapController.FromRealPosition(temp);
        }
        //ObjExcel.UserControl = true;
    }
}