using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    public string variableName = "A_Volume";
    private Slider relatedSlider;

    private void Awake()
    {
        relatedSlider = GetComponentInChildren<Slider>();
    }

    public void Start()
    {
        relatedSlider.value = OptionsManager.Instance.GetMixerVariable(variableName);
        relatedSlider.onValueChanged.AddListener(UpdateMixer);
    }

    private void UpdateMixer(float newValue)
    {
        OptionsManager.Instance.EditMixerVariable(variableName, newValue);
    }
}
