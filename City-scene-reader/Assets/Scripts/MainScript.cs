using Mapbox.Utils;
using MyOwnClass;
using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{

    public static MainScript instance;  // синглтон

    public GameObject usedObject; //если что-то работать не будет, будешь его снизу создавать сразу, а не здесь
    public string nameOfFile;
    public Vector3 coordinatesForSpawn;
    public GameObject person;
    [SerializeField] Transform map;

    // пользовательские поля для ввода координат объекта x/y/z, размера объекта
    public InputField InputX;
    public InputField InputY;
    public InputField InputZ;
    public InputField InputSize;
    public TextMeshProUGUI data;
    // отображение/ввод описания 
    public Text description;
    public InputField field;

    public float mouseWheel;    //текущее значение колесика
    string nameOfSavedFile;     // название фала сохранения

    [SerializeField] float scaleDelta = 0.05f;   // дельта изменения размера

    public GameObject iconArrowsOfRotation;
    public GameObject iconArrowsOfMovement;

    /// <summary>
    /// Инициализация всех компанентов, зачистка ненужных 
    /// </summary>
    private void Awake()
    {
        instance = this;    // инициализация синглтона
        data.text = "1670";
        // рабочий 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        DifferentThings.CameraSpeed = 15.0f;
        DifferentThings.mouseSensitivity = 3;
        // инициализация статичных полей DifferentThings
        /*DifferentThings.mouseSensitivity = 0;*/
        /*DifferentThings.CameraSpeed = 0;*/
        DifferentThings.InputX = InputX;
        DifferentThings.InputY = InputY;
        DifferentThings.InputZ = InputZ;
        DifferentThings.InputSize = InputSize;
        DifferentThings.description = description;
        DifferentThings.MovementArrows = true;
        DifferentThings.IsDescriptionActive = false;
        mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        //зачистка рабочей папки
        if (Directory.Exists(Application.streamingAssetsPath + "/Streaming/"))
        {
            Directory.Delete(Application.streamingAssetsPath + "/Streaming/", true);
        }
        if (!Directory.Exists(Application.streamingAssetsPath + "/Streaming/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Streaming/");
        }
    }

    public static void OptimizeGameObject(GameObject oprimizingObject)
    {
        oprimizingObject.transform.parent = instance.map.transform;
        for (int i = 0; i < oprimizingObject.transform.childCount; i++)
        {
            oprimizingObject.transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
        }
        oprimizingObject.AddComponent<House>();
        oprimizingObject.AddComponent<MeshCollider>();
        oprimizingObject.AddComponent<Building>();
    }

    IEnumerator LoadEverythingCoroutine()
    {
        FileBrowser.SetFilters(false, ".streetV2");
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Open project", "Open");
        if (FileBrowser.Success)
        {
            DestroyEverything();
            nameOfSavedFile = FileBrowser.Result[0];
            lzip.decompress_File(nameOfSavedFile, Application.streamingAssetsPath + "/Streaming/");

            using (FileStream streetStream = new FileStream(Application.streamingAssetsPath + "/Streaming/" + "settings.information", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                DifferentThings.Objects = (List<ObjectOfConstructor>)formatter.Deserialize(streetStream);
                DifferentThings.Roads = (List<SerializableRoad>)formatter.Deserialize(streetStream);
                streetStream.Close();
            }

            float tempScaleY = MapController.instance.abstractMap.gameObject.transform.localScale.y;
            float tempScaleX = MapController.instance.abstractMap.gameObject.transform.localScale.x;

            MapController.instance.abstractMap.gameObject.transform.localScale = new Vector3(tempScaleX, tempScaleX, tempScaleX);

            foreach (ObjectOfConstructor loadedObject in DifferentThings.Objects)
            {
                usedObject = Load3dObjectByPathViaTriLib(Application.streamingAssetsPath + "/Streaming/" + loadedObject.nameOfModel);
                OptimizeGameObject(usedObject);

                usedObject.GetComponent<House>().Year = loadedObject.year;
                TimeMachine.instance.Houses.Add(usedObject.GetComponent<House>());

                usedObject.transform.position = MapController.FromRealPosition(loadedObject.position.ToVector3());
                usedObject.transform.localScale = new Vector3(loadedObject.size, loadedObject.size, loadedObject.size);
                usedObject.transform.rotation = loadedObject.rotation.ToQuaternion();

                MeshRenderer rd = usedObject.transform.GetChild(0).GetComponent<MeshRenderer>();
                switch (loadedObject.color)
                {
                    case 0:
                        break;
                    case 1:
                        rd.material = TimeMachine.instance.one;
                        break;
                    case 2:
                        rd.material = TimeMachine.instance.two;
                        break;
                    case 3:
                        rd.material = TimeMachine.instance.three;
                        break;
                    case 4:
                        rd.material = TimeMachine.instance.four;
                        break;
                    case 5:
                        rd.material = TimeMachine.instance.five;
                        break;
                    case 6:
                        rd.material = TimeMachine.instance.six;
                        break;
                    case 7:
                        rd.material = TimeMachine.instance.seven;
                        break;
                }

                usedObject.GetComponent<Building>().description = loadedObject.description;


                DifferentThings.allRealObjects.Add(usedObject);
            }
            TimeMachine.instance.InstantiateOnData();

            foreach (SerializableRoad item in DifferentThings.Roads.ToArray())
            {
                RoadEditor.instance.BuildRoad(item.startPosition.ToVector3(), item.endPosition.ToVector3(), item.width, item.hight, item.name);
            }

            MapController.instance.abstractMap.gameObject.transform.localScale = new Vector3(tempScaleX, tempScaleY, tempScaleX);
        }
    }
    public static GameObject Load3dObjectByPathViaTriLib(string path)
    {
        AssetLoaderContext assetLoaderContext = AssetLoader.LoadModelFromFileNoThread(path, null, null);
        GameObject result = assetLoaderContext.RootGameObject;
        return result;
    }
    public void LoadEverything()
    {
        StartCoroutine(LoadEverythingCoroutine());
    }

    public void DestroyEverything()
    {
        //GameObject.FindGameObjectWithTag("AllArrows").transform.localScale = new Vector3(0, 0, 0);
        //GameObject.FindGameObjectWithTag("ArrowsOfRotation").transform.localScale = new Vector3(0, 0, 0);

        for (int i = DifferentThings.allRealObjects.Count - 1; i >= 0; i--)
        {

            Destroy(DifferentThings.allRealObjects[i]);
            DifferentThings.allRealObjects.RemoveAt(i);
            DifferentThings.Objects.RemoveAt(i);

        }
        RoadEditor.instance.ClearAllRoads();
        DifferentThings.Objects.Clear();
        DifferentThings.Roads.Clear();

        TimeMachine.instance.Houses.Clear();


        Directory.Delete(Application.streamingAssetsPath + "/Streaming/", true);
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Streaming/");
        //GameObject.Find("Panel").transform.localScale = new Vector3(0, 0, 0);
        //GameObject.Find("Оси").transform.localScale = new Vector3(0f, 0f, 0f); //прячем ручной ввод координат

    }

    public void ArrowsOfRotation()
    {
        DifferentThings.MovementArrows = false;
        iconArrowsOfMovement.SetActive(false);
        iconArrowsOfRotation.SetActive(true);
        if (DifferentThings.IsThereAnyActiveObjects)
        {
            ArrowsController.instance.arrowsPosition.SetActive(false);
            ArrowsController.instance.arrowsRotation.SetActive(true);
            ArrowsController.instance.UseArrows();
        }
        ArrowsController.instance.HideAllArrows();
        ArrowsController.instance.UpdateText();
    }

    public void ArrowsOfMovement()
    {
        DifferentThings.MovementArrows = true;
        iconArrowsOfMovement.SetActive(true);
        iconArrowsOfRotation.SetActive(false);
        if (DifferentThings.IsThereAnyActiveObjects)
        {
            ArrowsController.instance.arrowsPosition.SetActive(true);
            ArrowsController.instance.arrowsRotation.SetActive(false);
            ArrowsController.instance.UseArrows();
        }
        ArrowsController.instance.UpdateText();
    }
    
    // Update is called once per frame
    void Update()
    {
        /*//Ctrl для курсора
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            DifferentThings.mouseSensitivity = 3;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            DifferentThings.CameraSpeed = 15.0f;

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            DifferentThings.mouseSensitivity = 0;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            DifferentThings.CameraSpeed = 0;
            UnityEngine.Cursor.visible = true;
        }*/

        //Пробел для перемещения к объекту

        if (Input.GetKeyUp(KeyCode.Delete))
        {

        }

        //Shift для ускорения
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DifferentThings.CameraSpeed = 75;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            DifferentThings.CameraSpeed = 15;
        }

    }
}
