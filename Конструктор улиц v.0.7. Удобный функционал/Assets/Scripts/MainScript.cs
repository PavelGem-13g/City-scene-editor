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

        // инициализация статичных полей DifferentThings
        DifferentThings.mouseSensitivity = 0;
        DifferentThings.CameraSpeed = 0;
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
    /// <summary>
    /// Куратина загрузки объектов расширения
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCoroutine()
    {
        FileBrowser.SetFilters(false, ".obj", ".fbx", ".stl", ".3ds",".blend");
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Spawn object", "Spawn");
        if (FileBrowser.Success)
        {
            nameOfFile = FileBrowser.Result[0];
            Directory.CreateDirectory(Application.streamingAssetsPath + "/Streaming/");
            File.Copy(nameOfFile, Application.streamingAssetsPath + "/Streaming/" + Path.GetFileName(nameOfFile), true);
            if (File.Exists(nameOfFile.Replace(Path.GetExtension(nameOfFile), ".mtl")))
            {
                File.Copy(nameOfFile.Replace(Path.GetExtension(nameOfFile), ".mtl"), Application.streamingAssetsPath + "/Streaming/" + Path.GetFileName(nameOfFile.Replace(Path.GetExtension(nameOfFile), ".mtl")), true);
            }


            usedObject = Load3dObjectByPathViaTriLib(nameOfFile);
            OptimizeGameObject(usedObject);
            usedObject.transform.position = person.transform.position;

            DifferentThings.allRealObjects.Add(usedObject);

            DifferentThings.Objects.Add(new ObjectOfConstructor(
                MapController.GetRealPosition(usedObject.transform.position),
                usedObject.transform.rotation,
                Path.GetFileName(nameOfFile),
                usedObject.transform.localScale.x,
                ""));
        }
    }

    /// <summary>
    /// Запуск куратины спавна 3D объекта
    /// </summary>
    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    public static void OptimizeGameObject(GameObject oprimizingObject)
    {
        oprimizingObject.transform.parent = instance.map.transform;
        for (int i = 0; i < oprimizingObject.transform.childCount; i++)
        {
            oprimizingObject.transform.GetChild(i).gameObject.AddComponent<MeshCollider>();
        }

        oprimizingObject.AddComponent<MeshCollider>();
        oprimizingObject.AddComponent<Building>();
    }
    public void ChangeDescription()
    {
        DifferentThings.Objects[DifferentThings.numberOfActiveObject].description = field.text;
        ArrowsController.instance.activeObject.description = field.text;
        description.text = field.text;
        field.text = "Введите описание";
    }

    IEnumerator SaveEverythingCoroutine()
    {
        FileBrowser.SetFilters(false, ".streetV2");
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.FilesAndFolders, false, null, null, "Save project", "Save");
        if (FileBrowser.Success)
        {
            nameOfSavedFile = FileBrowser.Result[0];
            using (FileStream streetStream = new FileStream(Application.streamingAssetsPath + "/Streaming/" + "settings.information", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(streetStream, DifferentThings.Objects);
                formatter.Serialize(streetStream, DifferentThings.Roads);
                streetStream.Close();
            }
            lzip.compressDir(Application.streamingAssetsPath + "/Streaming/", 5, nameOfSavedFile);
        }
    }

    public void SaveEverything()
    {
        StartCoroutine(SaveEverythingCoroutine());
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

                usedObject.transform.position = MapController.FromRealPosition(loadedObject.position.ToVector3());
                usedObject.transform.localScale = new Vector3(loadedObject.size, loadedObject.size, loadedObject.size);
                usedObject.transform.rotation = loadedObject.rotation.ToQuaternion();

                usedObject.GetComponent<Building>().description = loadedObject.description;


                DifferentThings.allRealObjects.Add(usedObject);
            }

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
    GameObject Load3dObjectByPathViaLoader3DS(string path)
    {
        GameObject result = new GameObject();
        Loader3DS loader3DS = result.AddComponent<Loader3DS>();
        loader3DS.Load(path);
        Destroy(loader3DS);
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

        description.text = null;


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

    public void OpenDescription()
    {
        DifferentThings.IsDescriptionActive = true;
        GameObject.Find("CloseDescription").transform.localScale = new Vector2(1, 5.1f);
        GameObject.Find("DescriptionButton").transform.localScale = new Vector2(0, 0);

        if (DifferentThings.IsThereAnyActiveObjects)
        {
            description.text = DifferentThings.Objects[DifferentThings.numberOfActiveObject].description;
            GameObject.Find("Panel").transform.localScale = new Vector2(14, 2.87f);

        }
    }

    public void CloseDescription()
    {
        DifferentThings.IsDescriptionActive = false;

        GameObject.Find("Panel").transform.localScale = new Vector2(0, 0);
        GameObject.Find("CloseDescription").transform.localScale = new Vector2(0, 0);
        GameObject.Find("DescriptionButton").transform.localScale = new Vector2(1, 5.1f);
    }
    public void ChangeAllCoordinates()
    {
        if (DifferentThings.MovementArrows)
        {
            try
            {
                Vector3 temp = MapController.instance.abstractMap.GeoToWorldPosition(new Vector2d(Convert.ToDouble(InputX.text), Convert.ToDouble(InputZ.text)), false);
                temp.y = (float)MapController.instance.abstractMap.QueryElevationInUnityUnitsAt(new Vector2d(Convert.ToDouble(InputX.text), Convert.ToDouble(InputZ.text))) * MapController.YScale *
                    ((float)Convert.ToDouble(InputY.text) /
                         MapController.instance.abstractMap.QueryElevationInMetersAt(new Vector2d(Convert.ToDouble(InputX.text), Convert.ToDouble(InputZ.text))));

                DifferentThings.Objects[DifferentThings.numberOfActiveObject].position = new SerializableVector3(MapController.GetRealPosition(temp));
                ArrowsController.instance.activeObject.transform.position = temp;

                ArrowsController.instance.UseArrows();
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                ArrowsController.instance.activeObject.transform.rotation = Quaternion.Euler(
                    (float)((float)Convert.ToDouble(InputX.text)),
                    (float)((float)Convert.ToDouble(InputY.text)),
                    (float)((float)Convert.ToDouble(InputZ.text)));//Вращаем
                ArrowsController.instance.arrowsRotation.transform.rotation = Quaternion.Euler(
                    (float)((float)Convert.ToDouble(InputX.text)),
                    (float)((float)Convert.ToDouble(InputY.text)),
                    (float)((float)Convert.ToDouble(InputZ.text)));
                DifferentThings.Objects[DifferentThings.numberOfActiveObject].rotation = new SerializableQuaternion(ArrowsController.instance.activeObject.transform.rotation);
            }
            catch
            {

            }
        }
    }

    public void ChangeSize()
    {
        try
        {
            DifferentThings.Objects[DifferentThings.numberOfActiveObject].size = (float)Convert.ToDouble(InputSize.text);
            ArrowsController.instance.activeObject.transform.localScale = new Vector3((float)Convert.ToDouble(InputSize.text), (float)Convert.ToDouble(InputSize.text), (float)Convert.ToDouble(InputSize.text));
        }
        catch
        {
            InputSize.text = ArrowsController.instance.activeObject.transform.localScale.x.ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        //Ctrl для курсора
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
        }

        //Пробел для перемещения к объекту
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (DifferentThings.IsThereAnyActiveObjects)
            {
                GameObject.Find("Main Camera").transform.position = new Vector3(DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.position.x, DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.position.y + 30, DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.position.z);
            }
        }



        //Удаление объекта
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject]);
            DifferentThings.allRealObjects.RemoveAt(DifferentThings.numberOfActiveObject);
            DifferentThings.Objects.RemoveAt(DifferentThings.numberOfActiveObject);

            ArrowsController.instance.HideAllArrows();
            //GameObject.FindGameObjectWithTag("AllArrows").transform.localScale = new Vector3(0, 0, 0);
            //GameObject.FindGameObjectWithTag("ArrowsOfRotation").transform.localScale = new Vector3(0, 0, 0);
            description.text = "";
            //GameObject.Find("Panel").transform.localScale = new Vector3(0, 0, 0);
            //GameObject.Find("Оси").transform.localScale = new Vector3(0f, 0f, 0f); //прячем ручной ввод координат
        }

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

        //Колёсико мыши


        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel > 0 && ArrowsController.instance.activeObject != null)
        {
            DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale += new Vector3(scaleDelta, scaleDelta, scaleDelta);
            DifferentThings.Objects[DifferentThings.numberOfActiveObject].size = DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale.x;
            InputSize.text = Convert.ToString(DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale.x);
        }

        if (mouseWheel < 0 && ArrowsController.instance.activeObject != null)
        {
            DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale -= new Vector3(scaleDelta, scaleDelta, scaleDelta);
            DifferentThings.Objects[DifferentThings.numberOfActiveObject].size = DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale.x;
            InputSize.text = Convert.ToString(DifferentThings.allRealObjects[DifferentThings.numberOfActiveObject].transform.localScale.x);
        }
    }
}
