using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public TextMeshProUGUI textValue;
    private Slider relatedSlider;

    private void Awake()
    {
        relatedSlider = GetComponentInChildren<Slider>();
    }

    public void Start()
    {
        relatedSlider.value = OptionsManager.Instance.GetSensitivity();
        SetNumericValue(relatedSlider.value);
        relatedSlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void UpdateSensitivity(float newValue)
    {
        SetNumericValue(newValue);
        OptionsManager.Instance.EditSensitivity(newValue);
    }

    private void SetNumericValue(float value)
    {
        float correctedValue = value;
        textValue.text = correctedValue.ToString("f1"); 
    }
}

