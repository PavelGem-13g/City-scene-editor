using UnityEngine;
using UnityEngine.UI;

public class ChangeSteetBuildings : MonoBehaviour
{
    public InputField inputField;

    private void Awake()
    {
        inputField = GetComponentInChildren<InputField>();
    }
    public void Submit()
    {
        if (ArrowsController.instance.activeObject != null)
        {
            ArrowsController.instance.activeObject.streetName = inputField.text;
        }
    }
    public void Activate()
    {
        if (ArrowsController.instance.activeObject != null)
        {
            inputField.text = ArrowsController.instance.activeObject.streetName;
        }
    }
}
