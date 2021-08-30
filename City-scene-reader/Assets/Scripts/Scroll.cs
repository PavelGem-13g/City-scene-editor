using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Scroll : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider _slider;
    public TextMeshProUGUI _sliderValue;
    void Start()
    {
        
    }

    public void OnScroll()
    {
        _sliderValue.text = _slider.value.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
