using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mark : MonoBehaviour, IDragHandler,IPointerDownHandler
{
    public RectTransform rectTransform;
    public InputField inputHight;
    public int hight;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        hight = 0;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        if (rectTransform.transform.localPosition.x > rectTransform.transform.parent.GetComponent<RectTransform>().rect.width / 2)
        {
            rectTransform.transform.localPosition = new Vector3(rectTransform.transform.parent.GetComponent<RectTransform>().rect.width / 2, rectTransform.transform.localPosition.y);
        }
        if (rectTransform.transform.localPosition.x < -rectTransform.transform.parent.GetComponent<RectTransform>().rect.width / 2)
        {
            rectTransform.transform.localPosition = new Vector3(-rectTransform.transform.parent.GetComponent<RectTransform>().rect.width / 2, rectTransform.transform.localPosition.y);
        }
        if (rectTransform.transform.localPosition.y > rectTransform.transform.parent.GetComponent<RectTransform>().rect.height / 2)
        {
            rectTransform.transform.localPosition = new Vector3(rectTransform.transform.localPosition.x, rectTransform.transform.parent.GetComponent<RectTransform>().rect.height / 2);
        }
        if (rectTransform.transform.localPosition.y < -rectTransform.transform.parent.GetComponent<RectTransform>().rect.height / 2)
        {
            rectTransform.transform.localPosition = new Vector3(rectTransform.transform.localPosition.x, -rectTransform.transform.parent.GetComponent<RectTransform>().rect.height / 2);
        }
        GroundEditor.instance.UpdateRealPosition(rectTransform.transform.localPosition);
        GroundEditor.instance.UpdateLines();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            GroundEditor.instance.DeleteMarker(this);
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonDown(2))
        {
            GroundEditor.instance.SetMarker(this);
        }
        GroundEditor.instance.UpdateRealPosition(rectTransform.transform.localPosition);
    }
    public void ChangeValue() 
    {
        hight = Convert.ToInt32(inputHight.text);
    }
}
