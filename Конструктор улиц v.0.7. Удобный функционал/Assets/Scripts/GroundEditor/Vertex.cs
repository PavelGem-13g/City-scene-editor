using UnityEngine;

namespace Assets.Scripts.GroundEditor
{
    public class Vertex : MonoBehaviour
    {
        Mark _mark;

        public void Destroy() 
        {
            Destroy(gameObject);
        }

        public void SetMark(Mark mark) 
        {
            _mark = mark;
        }
    }
}
