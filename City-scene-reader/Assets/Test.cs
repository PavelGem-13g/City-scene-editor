using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(MapController.FromRealPosition(new Vector3(55.75f, 1f, 37.63f)));
    }
}
