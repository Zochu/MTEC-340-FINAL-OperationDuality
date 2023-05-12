using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingBar : MonoBehaviour
{

    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetValue(float val)
    {
        slider.value = val;
    }

    public void SetMax(float val)
    {
        slider.maxValue = val;
    }
}
