using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YearVisualiser : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        SetValue();
    }

    public void SetValue()
    {
        textMeshProUGUI.SetText(Convert.ToString(slider.value));
    }
}
