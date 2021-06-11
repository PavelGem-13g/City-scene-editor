using UnityEngine;
using UnityEngine.EventSystems;

public class Line : MonoBehaviour, IPointerDownHandler
{
    public RectTransform rectTransform;

    Mark _startMark;
    Mark _endMark;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetWidth(float width)
    {
        rectTransform.localScale = new Vector3(width / 100f, 0.01f, 0.01f);
    }
    public void SetAngle(float angle)
    {
        rectTransform.rotation = Quaternion.EulerAngles(0, 0, angle);
    }
    public void SetLine(Mark startMark, Mark endMark)
    {
        _startMark = startMark;
        _endMark = endMark;
    }

    public void DrawLine()
    {
        rectTransform.localPosition = (StartPosition + EndPosition) / 2;
        SetWidth(Vector3.Distance(StartPosition, EndPosition));
        SetAngle(Mathf.Atan(
           (StartPosition.y - EndPosition.y) /
           (StartPosition.x - EndPosition.x)));
    }

    public bool IsContainsMarker(Mark mark)
    {
        return _startMark == mark || _endMark == mark;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Delete();
        }
    }
    public void Delete()
    {
        GroundEditor.instance.lines.Remove(this);
        Destroy(gameObject);
    }

    public Vector3 StartPosition 
    {
        get 
        {
            return _startMark.rectTransform.localPosition;
        }
    }
    public Vector3 EndPosition
    {
        get
        {
            return _endMark.rectTransform.localPosition;
        }
    }
}
