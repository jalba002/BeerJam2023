using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public string variableName = "A_Volume";
    public TextMeshProUGUI textValue;
    private Slider relatedSlider;

    private void Awake()
    {
        relatedSlider = GetComponentInChildren<Slider>();
    }

    public void Start()
    {
        relatedSlider.value = OptionsManager.Instance.GetMixerVariable(variableName);
        SetNumericValue(relatedSlider.value);
        relatedSlider.onValueChanged.AddListener(UpdateMixer);
    }

    private void UpdateMixer(float newValue)
    {
        OptionsManager.Instance.EditMixerVariable(variableName, newValue);
        SetNumericValue(newValue);
    }

    private void SetNumericValue(float value)
    {
        float correctedValue = value*100f;
        textValue.text = correctedValue.ToString("f0");
    }
}
